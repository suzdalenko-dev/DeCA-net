using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class EmailSender
{
    private readonly IConfiguration _configuration;
    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendLoginLinkAsync(string email, string link)
    {
        var message = new MimeMessage();

        /*
        message.From.Add(
            MailboxAddress.Parse(_configuration["Email:From"]!)
        );
        */

        message.From.Add(
            new MailboxAddress("DeCA", "suzdalenko.dev@gmail.com")
        );

        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = "Acceso a DeCA";
        message.Body = new BodyBuilder
        {
            HtmlBody = $"""
                <div style="font-family:Arial,sans-serif;max-width:500px;margin:auto">
                    <h1>DeCA</h1>
                    <p>Has solicitado acceder a tu cuenta.</p>
                    <p>
                        <a href="{link}"
                           style="background:#107749;color:white;
                           padding:14px 25px;text-decoration:none;
                           border-radius:5px;display:inline-block">
                           Acceder a DeCA
                        </a>
                    </p>
                    <p>Este enlace será válido durante 11 días.</p>
                    <p>Si no has solicitado este acceso, puedes ignorar este correo.</p>
                </div>
                """
        }.ToMessageBody();

        using var smtp = new SmtpClient();
        /*
        await smtp.AuthenticateAsync(
            _configuration["Email:Username"],
            _configuration["Email:Password"]
        );
        */
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            "suzdalenko.dev@gmail.com",
            _configuration["Email:Password"]
        );

        await smtp.SendAsync(message);

        await smtp.DisconnectAsync(true);
    }
}

/*
Email__Password = TU_CONTRASEÑA_DE_APLICACION
dotnet user-secrets init
dotnet user-secrets set "Email:Password" "TU_CONTRASEÑA_DE_APLICACION"
*/