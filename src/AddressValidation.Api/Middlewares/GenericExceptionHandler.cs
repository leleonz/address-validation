namespace AddressValidation.Api.Middlewares
{
    //Change class to abstract when having more detail exception to handle
    /// <summary>
    /// Catch all exceptions, log and modify response before sending
    /// </summary>
    public class GenericExceptionHandler : IMiddleware
    {
        private readonly ILogger<GenericExceptionHandler> _logger;
        private readonly IHostEnvironment _environment;

        public GenericExceptionHandler(ILogger<GenericExceptionHandler> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public virtual async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;

                //Example to consume method to handle exception
                //await HandleExceptionAsync(context, ex);
            }
        }

        /* 
        //Uncomment if making into abstract
        public abstract async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //Example implementation
            //context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //var response = _environment.IsDevelopment()
              //  ? new CustomError(ex.Message, ex.StackTrace)
              //  : new CustomError(ex.Message);

            //var jsonResponse = JsonSerializer.Serialize(response);
            //await context.Response.WriteAsJsonAsync(jsonResponse);
        }
        */
    }

    /*
    //sample custom error model
    public class CustomError
    {
        public string Message { get; set; }
        public string? Details { get; set; }

        public CustomError(string message) : this(message, string.Empty) { }

        public CustomError(string message, string? details)
        {
            Message = message;
            Details = details;
        }
    }*/
}
