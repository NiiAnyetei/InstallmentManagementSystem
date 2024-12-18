using System.Net;
using System.Text;

namespace IMS.Config
{
    public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration config;

        public SwaggerBasicAuthMiddleware(RequestDelegate next, IConfiguration config)
        {
            this.next = next;
            this.config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    var username = decodedUsernamePassword.Split(':', 2)[0];
                    var password = decodedUsernamePassword.Split(':', 2)[1];

                    if (IsAuthorized(username, password))
                    {
                        await next.Invoke(context);
                        return;
                    }
                }

                context.Response.Headers["WWW-Authenticate"] = "Basic";

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await next.Invoke(context);
            }
        }

        public bool IsAuthorized(string username, string password)
        {
            var _userName = config["BasicAuth:User"];
            var _password = config["BasicAuth:Pass"];

            return username.Equals(_userName, StringComparison.InvariantCultureIgnoreCase) && password.Equals(_password);
        }
    }
}
