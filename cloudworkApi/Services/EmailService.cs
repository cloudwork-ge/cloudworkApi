using System;
using System.Net;
using System.Net.Mail;

namespace cloudworkApi.Services
{
    public class EmailService
    {
        public EmailService()
        {
            
        }
        public void SendEmail(string To, string Subject, string Body)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(To));
                message.From = new MailAddress("info@ants.ge", "ants.ge");
                //message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                //message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    //client.Credentials = new NetworkCredential("nightwishhhhh@gmail.com", "ccucysmuefnvutpa");
                    //client.Credentials = new NetworkCredential("info@ants.ge", "molrewlxlkomvgxt");
                    client.Credentials = new NetworkCredential("crm@ants.ge", "isqtmstiuifctuoy");

                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }
    }
}
