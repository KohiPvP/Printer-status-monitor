using System;
using System.Net;
using System.Net.Mail;

class EmailSender
{
    static void Main(string[] args)
    {
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("barazso@hilti.com");
            mail.To.Add("barazso@hilti.com");
            mail.To.Add("toerkri@hilti.com");
            mail.Subject = "Teszt email";
            mail.Body = "Ez egy teszt üzenet C#-ból.";

            SmtpClient smtpServer = new SmtpClient("mailhost.hilti.com");
            smtpServer.Port = 25;
            smtpServer.Credentials = new NetworkCredential("barazso@hilti.com", "Adgjmpt2004@");
            smtpServer.EnableSsl = false;

            smtpServer.Send(mail);
            Console.WriteLine("Email sikeresen elküldve!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hiba: " + ex.Message);
        }
    }
}
