namespace SQLicious_ASP.NET_MVC.Options.Email.IEmail
{
    public interface IEmailSender
    {
        public Task<EmailResult> SendEmailAsync(string email, string subject, string body);
    }
}
