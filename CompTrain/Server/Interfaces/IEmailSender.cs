using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CompTrain.Server.Interfaces
{
    public interface IEmailSender
    {
        List<MailAddress> GetEmailList(string emails);
        bool IsValidEmail(string email);
        Task<bool> SendMail(string To, string Subject, string Body, string Allegati = null, string Cc = null);
    }
}