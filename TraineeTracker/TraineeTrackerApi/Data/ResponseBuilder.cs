using Microsoft.AspNetCore.Mvc;
using TraineeTrackerApp.Models;

namespace TraineeTrackerApi.Data;

public static class ResponseBuilder
{
    public static object GetResponseLink(HttpRequest request)
    {
        return new
        {
            self = $"https://{request.Host.Value}{request.Path.Value}"
        };
    }


    public static object GetResponse_Error_NoAccessToken()
    {
        return new
        {
            error = new
            {
                message = "Invalid Request",
                details = "Ensure that the header contains the following 'Access-Token: {access-token}'"
            }
        };
    }

    public static object GetResponse_Error_NoAccess()
    {
        return new
        {
            error = new
            {
                message = "Unauthorised Request",
                details = "You do not have the permissions to view this resource"
            }
        };
    }

    public static object GetResponse_Error_NotFound()
    {
        return new
        {
            error = new
            {
                message = "Resource Not Found",
                details = "The specified resource doesn't exists make sure you are using correct id"
            }
        };
    }
}