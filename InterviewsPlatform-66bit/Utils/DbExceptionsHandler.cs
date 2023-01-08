using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.GridFS;

namespace InterviewsPlatform_66bit.Utils;

public static class DbExceptionsHandler
{
    public static async Task<IActionResult> HandleAsync(Func<Task<IActionResult>> handling,
        IActionResult badRequest, IActionResult notFound)
    {
        try
        {
            return await handling();
        }
        catch (Exception ex) when (ex is FormatException or IndexOutOfRangeException)
        {
            return badRequest;
        }
        catch (Exception ex) when (ex is InvalidOperationException or GridFSFileNotFoundException)
        {
            return notFound;
        }
    }
}