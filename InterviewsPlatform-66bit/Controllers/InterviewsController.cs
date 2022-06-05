using InterviewsPlatform_66bit.DB;
using InterviewsPlatform_66bit.DTO;
using InterviewsPlatform_66bit.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace InterviewsPlatform_66bit.Controllers;

[Authorize]
[Route("/interviews/{id}")]
public class InterviewsController : Controller
{
    private readonly IDBResolver dbResolver;
    private readonly string dbName;

    public InterviewsController(IDBResolver dbResolver, string dbName)
    {
        this.dbResolver = dbResolver;
        this.dbName = dbName;
    }

    [HttpPatch]
    [Route("time-stops")]
    public async Task<IActionResult> AddTimeStops(string id, [FromBody] DateTime[] times)
    {
        var interviewsCollection = dbResolver.GetMongoCollection<InterviewDTO>(dbName, "interviews");

        return await DbExceptionsHandler.HandleAsync(async () =>
        {
            var filter = Builders<InterviewDTO>.Filter.Eq(i => i.Id, id);
            var update = Builders<InterviewDTO>.Update.PushEach(i => i.TimeStops, times);

            var updateRes = await interviewsCollection.FindOneAndUpdateAsync(filter, update);

            return Ok(updateRes.TimeStops.Concat(times));
        }, BadRequest(), NotFound());
    }
}