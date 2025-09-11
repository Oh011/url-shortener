using Microsoft.EntityFrameworkCore;
using Project.Application.Exceptions;
using Shared;
using System.Net;

namespace Url_Shortener.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {


        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        private readonly RequestDelegate _next;



        public GlobalErrorHandlingMiddleware(ILogger<GlobalErrorHandlingMiddleware> logger, RequestDelegate next)
        {


            _logger = logger;
            _next = next;

        }


        public async Task InvokeAsync(HttpContext context)
        {


            try
            {

                await _next(context);
            }


            catch (Exception ex)
            {


                await HandleException(ex, context);
            }

        }


        public async Task HandleException(Exception exception, HttpContext context)
        {


            context.Response.ContentType = "application/json";



            if (exception is ValidationException e)
            {

                context.Response.StatusCode = 400;



                await context.Response.WriteAsJsonAsync
                (ApiResponseFactory.Failure(e.Message, e.errors, HttpStatusCode.BadRequest));

                return;

            }



            context.Response.StatusCode = exception switch
            {


                BadRequestException => (int)HttpStatusCode.BadRequest,

                NotFoundException => (int)HttpStatusCode.NotFound,




                DbUpdateConcurrencyException => (int)HttpStatusCode.Conflict,


                ForbiddenException => (int)HttpStatusCode.Forbidden,

                UnAuthorizedException => (int)HttpStatusCode.Unauthorized,

                AccountLockedException => (int)HttpStatusCode.Locked,

                ConflictException => (int)HttpStatusCode.Conflict,


                _ => (int)HttpStatusCode.InternalServerError,

            };


            var statusCode = (HttpStatusCode)context.Response.StatusCode;




            string message = statusCode switch
            {


                HttpStatusCode.InternalServerError =>
                    "An unexpected error occurred. Please try again later.",

                _ => exception.Message
            };


            var response = ApiResponseFactory.Failure(message, statusCode);





            await context.Response.WriteAsJsonAsync(response);

        }

    }
}


//{
//    "email": "ohisham011@gmail.com",
//  "password": "Om#12345",
//  "username": "omar011",

//}


//"accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im9tYXIwMTEiLCJlbWFpbCI6Im9oaXNoYW0wMTFAZ21haWwuY29tIiwibmFtZWlkIjoiMzk1YzI2OTMtN2MwYi00ZGUwLWJkYTUtZDU5ZWZmYzc3YjQ2Iiwicm9sZSI6IlVzZXIiLCJuYmYiOjE3NTc0MDc4MTMsImV4cCI6MTc1NzQxMTQxMywiaWF0IjoxNzU3NDA3ODEzLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDg0IiwiYXVkIjoiQXVkaWVuY2UifQ.zQi2ydaLmx5Aei8ZRiJrrDQ1yTmXPipdmHeR9sgRdt0"