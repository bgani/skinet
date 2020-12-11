using System;

namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            // ?? null coalescing operator. If the value is null, execute what is to the right of these question marks.
            Message = message ?? GetDefaultMessageStatusCode(statusCode);
        }

        public int StatusCode { get; set; } 
        public string Message { get; set; }

        private string GetDefaultMessageStatusCode(int statusCode)
        {
            // instead of writing switch and using the case, break statements 
            // we can literlly put in the status code that we're looking for at an arrow, 
            // and the return what message we want to return
            // in default case we use underscore _
            return statusCode switch 
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Error are the path to the dark side. Error leads to anger. Anger leads to hate. Hate lead to career change.",
                _ => null
            };
        }
    }
}