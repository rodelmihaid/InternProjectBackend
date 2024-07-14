namespace InternProjectBackEnd
{
    using MailKit.Net.Smtp;
    using MimeKit;
    using System.Threading.Tasks;

    public class EmailService
    {
        public async Task SendEmailAsync(string email, string password)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("InternApp", "rodelutz@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Welcome to InternApp";

            string body = $@"
            <h2>Welcome aboard,</h2>
            <p>Emailul dvs. de conectare la platforma <strong>internApp.ro</strong> este <strong>{email}</strong>.</p>
            <p>Parola dvs. este <strong>{password}</strong>.</p>
            <p>Vă rugăm să vă conectați și să schimbați parola după prima autentificare.</p>
            <br>
            <p>Cu stimă,</p>
            <p>Echipa InternApp</p>";

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false); // Înlocuiește cu detaliile serverului tău SMTP
                client.Authenticate("rodelutz@gmail.com", "clch pfxi ghwj tokq"); // Înlocuiește cu datele tale de autentificare

                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }


}
