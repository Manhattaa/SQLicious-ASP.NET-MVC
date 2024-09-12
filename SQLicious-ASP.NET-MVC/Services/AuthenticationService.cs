using SQLicious_ASP.NET_MVC.Data.Repository;
using SQLicious_ASP.NET_MVC.Models;

namespace SQLicious_ASP.NET_MVC.Services
{
    public class AuthenticationService
    {
        private readonly JwtRepository _jwtRepository;

        public AuthenticationService(JwtRepository jwtRepository)
        {
            _jwtRepository = jwtRepository;
        }

        public string GenerateToken(Admin admin)
        {
            try
            {
                return _jwtRepository.GenerateJwt(admin);
            }

            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while generating the JWT.", ex);
            }
        }
    }
}
