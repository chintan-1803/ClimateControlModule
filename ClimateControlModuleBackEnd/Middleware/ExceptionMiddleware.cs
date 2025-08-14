using System.Net;

namespace ClimateControlModuleBackEnd.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private static readonly object _lock = new();


        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogError(context, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task LogError(HttpContext httpContext, Exception ex)
        {
            if (ex.StackTrace != null) 
            {
                var routeData = httpContext.GetRouteData();
                var controllerName = routeData.Values["controller"]?.ToString();
                var actionName = routeData.Values["action"]?.ToString();

                if (controllerName != null && actionName != null)
                {
                    await LogToFileAsync("Error occured in controller: " + controllerName + " in action called: " + actionName + "- " + ex.StackTrace);
                }
                else
                {
                    await LogToFileAsync(ex.StackTrace);
                }
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var result = new
            {
                error = "An unexpected error occurred.",
                details = ex.Message
            };
            return context.Response.WriteAsJsonAsync(result);
        }

        public Task LogToFileAsync(string message)
        {
            var logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
            Directory.CreateDirectory(logDirectory);

            var logFileName = $"log-{DateTime.UtcNow:yyyy-MM-dd}.txt";
            var logFilePath = Path.Combine(logDirectory, logFileName);

            var logEntry = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";

            lock (_lock)
            {
                // Daily Log file creation
                // If file does not exist, it will be created newly
                File.AppendAllText(logFilePath, logEntry); 
            }

            return Task.CompletedTask;
        }

    }
}
