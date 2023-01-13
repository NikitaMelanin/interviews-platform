﻿using InterviewsPlatform_66bit.DB;
using InterviewsPlatform_66bit.DTO;
using InterviewsPlatform_66bit.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace InterviewsPlatform_66bit.Controllers;

[Authorize(Roles = $"{Roles.ADMINISTRATOR},{Roles.HR}")]
[Route("/vacancies")]
public class VacanciesController : Controller
{
    private readonly IDBResolver dbResolver;
    private readonly string dbName;
    private readonly IMongoCollection<VacancyDTO> collection;

    public VacanciesController(IDBResolver dbResolver, string dbName)
    {
        this.dbResolver = dbResolver;
        this.dbName = dbName;
        
        collection = dbResolver.GetMongoCollection<VacancyDTO>(dbName, "vacancies");
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<IActionResult> Create([FromBody] VacancyPostDTO postDto)
    {
        var creatorId = User.Identity!.Name;

        var vacancy = new VacancyDTO(postDto) { CreatorId = creatorId! };

        await collection.InsertOneAsync(vacancy);

        return Created($"/vacancies/{vacancy.Id}", vacancy);
    }

    [HttpPatch]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> Update(string id, [FromBody] VacancyPostDTO postDto) 
        => await DbExceptionsHandler.HandleAsync(async () =>
        {
            var update = Builders<VacancyDTO>.Update
                .Set(v => v.Name, postDto.Name)
                .Set(v => v.Description, postDto.Description)
                .Set(v => v.Questions, postDto.Questions);

            var filter = Builders<VacancyDTO>.Filter.Eq(v => v.Id, id);
            
            var vacancy = (await collection.FindAsync(filter)).Single();

            if (vacancy.CreatorId != User.Identity!.Name!)
            {
                return Forbid();
            }

            await collection.UpdateOneAsync(filter, update);

            return Ok(vacancy);
        }, BadRequest(), NotFound(new {errorText = "Bad id"}));

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> CreatedByUserVacancies()
        => await DbExceptionsHandler.HandleAsync(async () =>
        {
            var userId = User.Identity!.Name!;

            var filter = Builders<VacancyDTO>.Filter.Eq(v => v.CreatorId, userId);

            var vacancies = (await collection.FindAsync(filter)).ToEnumerable();

            return Ok(vacancies);
        }, BadRequest(), NotFound());

    [HttpGet]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> Read(string id)
        => await DbExceptionsHandler.HandleAsync(async () =>
        {
            var filter = Builders<VacancyDTO>.Filter.Eq(v => v.Id, id);

            var vacancy = (await collection.FindAsync(filter)).Single();

            return Ok(vacancy);
        }, BadRequest(), NotFound(new {errorText = "Bad id"}));

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(string id) 
        => await DbExceptionsHandler.HandleAsync(async () =>
        {
            var filter = Builders<VacancyDTO>.Filter.Eq(v => v.Id, id);
            
            var vacancy = (await collection.FindAsync(filter)).Single();

            if (vacancy.CreatorId != User.Identity!.Name!)
            {
                return Forbid();
            }

            var interviewsCollection = dbResolver.GetMongoCollection<InterviewDTO>(dbName, "interviews");

            foreach (var interview in vacancy.Interviews)
            {
                await interviewsCollection.DeleteOneAsync(interview);
            }

            await collection.DeleteOneAsync(filter);

            return NoContent();
        }, BadRequest(), NotFound(new {errorText = "Bad id"}));
}