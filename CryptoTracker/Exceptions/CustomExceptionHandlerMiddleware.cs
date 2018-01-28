using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Exceptions
{
    public static class CustomExceptionHandlerMiddleware
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var ex = error.Error;

                        if (ex is CustomException)
                        {
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 400;
                            var specEx = (CustomException)ex;
                            await context.Response.WriteAsync(specEx.ToString(), Encoding.UTF8);
                        }
                    }
                });
            });
        }
    }
}
