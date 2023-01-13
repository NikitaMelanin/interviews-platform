using InterviewsPlatform_66bit.DB;
using InterviewsPlatform_66bit.DTO;
using InterviewsPlatform_66bit.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace InterviewsPlatform_66bit.Controllers;


[Authorize(Roles = $"{Roles.ADMINISTRATOR},{Roles.HR}")]
[Route("/interviewees")]
public class IntervieweesController: Controller
{
    private readonly IDBResolver dbResolver;
    private readonly string dbName;
    private readonly IMongoCollection<IntervieweeDTO> collection;

    public IntervieweesController(IDBResolver dbResolver, string dbName)
    {
        this.dbResolver = dbResolver;
        this.dbName = dbName;

        collection = dbResolver.GetMongoCollection<IntervieweeDTO>(dbName, "interviewees");
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> AllInterviewees()
        => await DbExceptionsHandler.HandleAsync(async () =>
        {
            var documents = await collection.FindAsync(_ => true);

            return Ok(await documents.ToListAsync());
        }, BadRequest(), NotFound());

    [HttpGet("{id}")]
    public async Task<IActionResult> Read(string id)
        => await DbExceptionsHandler.HandleAsync(async () =>
        {
            var documents = await collection.FindAsync(dto => dto.Id == id);

            return Ok(await documents.SingleAsync());
        }, BadRequest(), NotFound());
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
        => await DbExceptionsHandler.HandleAsync(async () =>
        {
            await collection.DeleteOneAsync(dto => dto.Id == id);

            return NoContent();
        }, BadRequest(), NotFound());
}