using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APITemplate.Models
{
    public class ApiError
    {
        private ModelStateDictionary modelState;

        public ApiError(ModelStateDictionary modelState)
        {
            Message = "Invalid parameters";
            Detail = modelState
                .FirstOrDefault(x => x.Value.Errors.Any()).Value.Errors
                .FirstOrDefault().ErrorMessage;
        }

        public string Message { get; set; }

        public string Detail { get; set; }
    }
}
