using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.UI.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // فقط برای تست، ایمیل واقعی ارسال نمی‌کنیم
            Console.WriteLine($"Email To: {email}, Subject: {subject}");
            return Task.CompletedTask;
        }
    }
}