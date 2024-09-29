namespace SQLicious_ASP.NET_MVC.Helpers
{
    public class JwtAuthorizationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Retrieve the JWT token from the HttpContext cookie
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JWTToken"];

            // Add the JWT token to the Authorization header, if it exists
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            // Continue with the request
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
