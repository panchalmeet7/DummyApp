using DummyApp.Entities.Data;
using DummyApp.Repository.Interface;
using System.Net.Mail;
using DummyApp.Entities.ViewModels;

namespace DummyApp.Repository.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DummyAppContext _dummyAppContext;
        public EmailRepository(DummyAppContext dummyAppContext)
        {
            _dummyAppContext = dummyAppContext;
        }

        public void SendEmail(string recipient, string subject, string body)
        {
            MailSendViewModel mail = new MailSendViewModel();
            mail.From = "meetpanchal194@gmail.com";
            mail.To = recipient;
            mail.Subject = subject;
            mail.Body = body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("meetpanchal194@gmail.com", "ksdqxndnbbsofpyz"); // ***use valid credentials***

            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Port = 587;
            try
            {
                smtp.Send(mail.From, mail.To, mail.Subject, mail.Body);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }

        }

    }
}
