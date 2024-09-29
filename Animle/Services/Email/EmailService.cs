using Animle.Classes;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Animle.Services.Email;

public class EmailService
{
    private readonly ConfigSettings _appSettings;

    public EmailService(IOptions<ConfigSettings> options)
    {
        _appSettings = options.Value;
    }

    public bool SendEmail(EmailDto emailDto)
    {
        var email = _appSettings.Email;
        var gSercret = _appSettings.GmailSecret;
        var emailMessage = new MimeMessage();
        
        if (emailDto.From != null)
        {
            emailMessage.From.Add(new MailboxAddress(emailDto.From, emailDto.Email));
        }

        emailMessage.To.Add(new MailboxAddress("", emailDto.To));
        emailMessage.Subject = emailDto.Subject;
        emailMessage.Body = new TextPart("plain") { Text = emailDto.Body };
        
        try
        {
            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate(email, gSercret);
            client.Send(emailMessage);
            client.Disconnect(true);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}