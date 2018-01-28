using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CryptoTracker.Middlewares
{
    /// <summary>
    /// Return bad request (status code 400) when model is null or ModelState = false
    /// It must be registered on StartUp.cs file: services.AddScoped<ValidateModelAttribute>()
    /// </summary>
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var code = "INVALID_REQUEST";
            var message = string.Empty;

            if (actionContext.ActionArguments.Any(kv => kv.Value == null))
            {
                message = "Model cannot be null";
                actionContext.Result = new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new { Code = code, Message = message }),
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ContentType = "application/json"
                };
                return;
            }

            if (actionContext.ModelState.IsValid == false)
            {
                message = GetErrorFromModel(actionContext.ModelState);
                actionContext.Result = new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new { Code = code, Message = message }),
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ContentType = "application/json"
                };
                return;
            }

            base.OnActionExecuting(actionContext);
        }

        private string GetErrorFromModel(ModelStateDictionary modelState)
        {
            var message = "";
            var errors = modelState.ToList().SelectMany(t => t.Value.Errors.Select(s => s.ErrorMessage + "\n")).ToList();
            message = string.Join(", ", errors);
            return message;
        }
    }
}
