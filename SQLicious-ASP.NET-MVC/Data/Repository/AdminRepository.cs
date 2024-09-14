using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLicious_ASP.NET_MVC.Data.Repository.IRepository;
using SQLicious_ASP.NET_MVC.Models;
using SQLicious_ASP.NET_MVC.Options;
using SQLicious_ASP.NET_MVC.Options.Email.IEmail;
using SQLicious_ASP.NET_MVC.Services;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace SQLicious_ASP.NET_MVC.Data.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _baseUrl = "https://localhost:7294";
        private readonly UserManager<Admin> _userManager;
        private readonly SignInManager<Admin> _signInManager;
        private readonly AuthenticationService _authService;
        private readonly IEmailSender _emailSender;
        private readonly RestaurantContext _restaurantContext;

        public AdminRepository(UserManager<Admin> userManager, SignInManager<Admin> signInManager, AuthenticationService authService, IEmailSender emailSender, RestaurantContext restaurantContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _emailSender = emailSender;
            _restaurantContext = restaurantContext;
        }

        // CreateAdminAsync - updated to match IAdminRepository
        public async Task<IdentityResult> CreateAdminAsync(Admin admin, string password)
        {
            if (string.IsNullOrEmpty(admin.UserName))
            {
                admin.UserName = admin.Email;  //setting the Email as the Username
            }

            IdentityResult createAdminResult = await _userManager.CreateAsync(admin, password);

            if (!createAdminResult.Succeeded)
                return IdentityResult.Failed(new IdentityError { Description = string.Join(", ", createAdminResult.Errors.Select(e => e.Description)) });

            // Generate email confirmation token
            string emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(admin);
            string callbackUrl = $"{_baseUrl}/confirm-email?userId={admin.Id}&code={Uri.EscapeDataString(emailConfirmationToken)}";

            string emailSubject = "SQLicious - Bara ett steg kvar!";
            string emailBody =
                $"<h2>Välkommen till SQLicious kära Kollega!</h2>" +
                $"<p>Tack för att du använder SQLicious! Det är bara ett steg kvar innan du kan börja använda appen.<br>" +
                $"Klicka på länken nedan för att verifiera din email.<br>" +
                $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Klicka här för att verifiera din email</a>.<br><br>" +
                $"Om du har problem med att klicka på länken, kopiera och klistra in URL:en nedan i din webbläsare: <br>" +
                $"{callbackUrl}<br><br><br>" +
                $"Hör av dig om du har några frågor eller feedback genom att svara på detta mail.<br><br>" +
                $"Allt gott,<br>" +
                $"SQLicious</p>";

            var sendEmailResult = await _emailSender.SendEmailAsync(admin.Email, emailSubject, emailBody);
            if (!sendEmailResult.Success)
                return IdentityResult.Failed(new IdentityError { Description = "Failed to send email: " + string.Join(", ", sendEmailResult.ErrorMessage) });

            return IdentityResult.Success;
        }

        // Get a specific Admin by ID
        public async Task<Admin> GetAdminById(int id)
        {
            return await _restaurantContext.Admins.FindAsync(id);
        }

        // Return all Admins
        public async Task<IEnumerable<Admin>> GetAllAdmins()
        {
            return await _restaurantContext.Admins.ToListAsync();
        }

        // Update an existing Admin
        public async Task<IdentityResult> UpdateAdminAsync(Admin admin)
        {
            var result = await _userManager.UpdateAsync(admin);
            await _restaurantContext.SaveChangesAsync();
            return result;
        }

        // Delete an Admin
        public async Task<IdentityResult> DeleteAdminAsync(string password, ClaimsPrincipal currentUser)
        {
            string? email = currentUser.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return IdentityResult.Failed(new IdentityError { Description = "No matching user found with the provided email." });

            Admin? user = await _restaurantContext.Admins.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "No matching user found." });

            if (!await _userManager.CheckPasswordAsync(user, password))
                return IdentityResult.Failed(new IdentityError { Description = "Invalid login credentials." });

            using var transaction = await _restaurantContext.Database.BeginTransactionAsync();
            try
            {
                IdentityResult deleteAccountResult = await _userManager.DeleteAsync(user);
                if (!deleteAccountResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return IdentityResult.Failed(new IdentityError { Description = "Something went wrong while attempting to delete the account." });
                }

                await transaction.CommitAsync();
                return IdentityResult.Success;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Admin login logic
        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            Admin? admin = await _userManager.FindByEmailAsync(email);
            if (admin == null)
                return LoginResult.Failed("No Admin found.");

            var loginResult = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: true);
            if (loginResult.Succeeded)
            {
                string token = _authService.GenerateToken(admin);
                return LoginResult.Successful(token);
            }
            if (loginResult.IsNotAllowed)
                return LoginResult.Failed("You must confirm your account to log in. Please check your email for a verification link.");
            if (loginResult.IsLockedOut)
                return LoginResult.Failed("The account is locked due to multiple failed attempts. Try again in a few minutes.");

            return LoginResult.Failed("Invalid login attempt.");
        }

        // Send email verification logic
        public async Task<IdentityResult> SendEmailVerificationAsync(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return IdentityResult.Failed(new IdentityError { Description = "Invalid user ID or verification code." });

            Admin? admin = await _userManager.FindByIdAsync(userId);
            if (admin == null)
                return IdentityResult.Failed(new IdentityError { Description = "No user found." });

            IdentityResult result = await _userManager.ConfirmEmailAsync(admin, Uri.UnescapeDataString(code));
            return result.Succeeded
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError { Description = "Failed to confirm email: " + string.Join(", ", result.Errors.Select(e => e.Description)) });
        }
        public async Task<Admin> GetAdminByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
