using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;

namespace IntentAPI.Abstractions
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var jwt = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            if (!jwt.IsNullOrEmpty())
            {
                try
                {
                    FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(jwt);
                    string uid = decodedToken.Uid;
                    context.Items["FirebaseUserId"] = uid;
                }
                catch (Exception)
                {
                    context.Response.StatusCode = 401;
                }
            }
            await _next(context);
        }
    }
}
