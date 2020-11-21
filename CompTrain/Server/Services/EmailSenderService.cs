using CompTrain.Server.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompTrain.Server.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly ILogger<EmailSenderService> _logger;
        private readonly string _from;
        private readonly bool _isAuthenticate;
        private readonly string _username;
        private readonly string _password;
        private readonly string _host;
        private readonly bool _ssl;
        private readonly int _port;
        private readonly bool _isBodyHTML;
        private readonly string _bcc;

        public EmailSenderService(IOptions<EmailSenderOption> options, ILogger<EmailSenderService> logger)
        {
            _from = options.Value.From;
            _isAuthenticate = options.Value.IsAuthenticate;
            _username = options.Value.Username;
            _password = options.Value.Password;
            _host = options.Value.Host;
            _ssl = options.Value.SSL;
            _port = options.Value.Port;
            _isBodyHTML = options.Value.IsBodyHTML;
            _bcc = options.Value.Bcc;

            _logger = logger;
        }

        public async Task<bool> SendMail(string To, string Subject, string Body, string Allegati = null, string Cc = null)
        {

            try
            {
                if (!IsValidEmail(_from) || String.IsNullOrEmpty(_host) || String.IsNullOrEmpty(To) || String.IsNullOrEmpty(Subject) || String.IsNullOrEmpty(Body))
                    return false;

                using MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_from);
                mailMessage.Subject = Regex.Replace(Subject, @"\t|\n|\r", " ");
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.Body = Body;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = _isBodyHTML;

                foreach (MailAddress mailAddress in GetEmailList(To))
                {
                    mailMessage.To.Add(mailAddress);
                }

                if (mailMessage.To.Count == 0)
                    return false;

                foreach (MailAddress mailAddress in GetEmailList(Cc))
                {
                    mailMessage.CC.Add(mailAddress);
                }

                foreach (MailAddress mailAddress in GetEmailList(_bcc))
                {
                    mailMessage.Bcc.Add(mailAddress);
                }

                if (!String.IsNullOrEmpty(Allegati))
                {
                    string[] listAllegati = Allegati.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string allegato in listAllegati)
                    {
                        if (File.Exists(allegato))
                        {
                            Attachment attachment = new Attachment(allegato);
                            mailMessage.Attachments.Add(attachment);
                        }
                    }
                }
                
                using SmtpClient smtpClient = new SmtpClient();

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Host = _host;
                smtpClient.EnableSsl = _ssl;
                smtpClient.Port = _port;
                if (_isAuthenticate)
                {
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = new NetworkCredential(mailMessage.From.Address, _password);
                }
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message + " - To: " + To);
                return false;
            }
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                if (String.IsNullOrEmpty(email))
                    return false;

                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public List<MailAddress> GetEmailList(string emails)
        {
            List<MailAddress> mailList = new List<MailAddress>();
            try
            {
                if (String.IsNullOrEmpty(emails))
                    return mailList;

                string[] emailList = emails.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string email in emailList)
                {
                    if (IsValidEmail(email))
                        mailList.Add(new MailAddress(email));
                }
                return mailList;
            }
            catch
            {
                return null;
            }
        }
    }
}
