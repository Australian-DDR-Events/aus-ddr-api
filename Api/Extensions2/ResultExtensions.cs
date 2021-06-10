using System.Collections.Generic;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AusDdrApi.Extensions
{
    public static class ResultExtensions
    {
        public static ActionResult<TResponse> ConvertToActionResult<TRequest, TResponse, TEntity>(
            this EndpointWithResponse<TRequest,TResponse,TEntity> controller, Result<TEntity> result)
        {
            return result.Status switch
            {
                ResultStatus.Ok => controller.Ok(controller.Convert(result)),
                ResultStatus.Error => controller.Problem(result.Errors.ToString()),
                ResultStatus.Forbidden => controller.Forbid(),
                ResultStatus.Invalid => controller.BadRequest(result.ValidationErrors.ToModelStateDictionary(controller)),
                ResultStatus.NotFound => controller.NotFound(),
                _ => controller.Problem("unhandled")
            };
        }
        
        public static ActionResult<TResponse> ConvertToActionResult<TResponse, TEntity>(
            this EndpointWithResponse<TResponse,TEntity> controller, Result<TEntity> result) 
        {
            return result.Status switch
            {
                ResultStatus.Ok => controller.Ok(controller.Convert(result)),
                ResultStatus.Error => controller.Problem(result.Errors.ToString()),
                ResultStatus.Forbidden => controller.Forbid(),
                ResultStatus.Invalid => controller.BadRequest(result.ValidationErrors.ToModelStateDictionary(controller)),
                ResultStatus.NotFound => controller.NotFound(),
                _ => controller.Problem("unhandled")
            };
        }
        
        public static ActionResult ConvertToActionResult<TRequest>(
            this EndpointWithoutResponse<TRequest> controller, IResult result) 
        {
            return result.Status switch
            {
                ResultStatus.Ok => controller.Ok(),
                ResultStatus.Error => controller.Problem(result.Errors.ToString()),
                ResultStatus.Forbidden => controller.Forbid(),
                ResultStatus.Invalid => controller.BadRequest(result.ValidationErrors.ToModelStateDictionary(controller)),
                ResultStatus.NotFound => controller.NotFound(),
                _ => controller.Problem("unhandled")
            };
        }
        
        public static ActionResult ConvertToActionResult(
            this EndpointWithoutResponse controller, IResult result)
        {
            return result.Status switch
            {
                ResultStatus.Ok => controller.Ok(),
                ResultStatus.Error => controller.Problem(result.Errors.ToString()),
                ResultStatus.Forbidden => controller.Forbid(),
                ResultStatus.Invalid => controller.BadRequest(result.ValidationErrors.ToModelStateDictionary(controller)),
                ResultStatus.NotFound => controller.NotFound(),
                _ => controller.Problem("unhandled")
            };
        }
        
        private static ModelStateDictionary ToModelStateDictionary(this IEnumerable<ValidationError> validationErrors, ControllerBase controller)
        {
            foreach (var error in validationErrors)
            {
                // TODO: Fix after updating to 3.0.0
                controller.ModelState.AddModelError(error.Identifier, error.ErrorMessage);
            }
            return controller.ModelState;
        }
    }
}