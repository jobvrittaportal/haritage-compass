using TourTravel.Configuration;
using TourTravel.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace TourTravel.Services
{
    public class MailService : IMailService
    {
        private readonly MailSetting _mailSettings;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MailService(IOptions<MailSetting> mailSettingsOptions, IWebHostEnvironment hostEnvironment)
        {
            _mailSettings = mailSettingsOptions.Value;
            this.webHostEnvironment = webHostEnvironment;
        }

        public bool SendMail(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom;
                    emailFrom = new MailboxAddress(mailData.SenderName != null ? mailData.SenderName : _mailSettings.SenderName, mailData.SenderEmail != null ? mailData.SenderEmail : _mailSettings.SenderEmail);

                    emailMessage.From.Add(emailFrom);
                    foreach (string EmailToId in mailData.EmailToId)
                    {
                        MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, EmailToId);
                        emailMessage.To.Add(emailTo);
                    }

                    if (mailData.EmailToCC != null)
                    {
                        foreach (string emailToCC in mailData.EmailToCC)
                        {
                            emailMessage.Cc.Add(new MailboxAddress("", emailToCC));
                        }
                    }

                    if (mailData.EmailToBCC != null)
                    {
                        foreach (string emailToBCC in mailData.EmailToBCC)
                        {
                            emailMessage.Bcc.Add(new MailboxAddress("", emailToBCC));
                        }
                    }

                    //   emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();


                    emailBodyBuilder.HtmlBody = mailData.EmailBody;
                    //string RootPath = this.webHostEnvironment.ContentRootPath;
                    if (mailData.Attachments != null)
                    {
                        foreach (string Attachment in mailData.Attachments)
                        {
                            if (File.Exists(Attachment))
                                emailBodyBuilder.Attachments.Add(Attachment);
                        }
                    }

                    if (mailData.AttachmentFile != null && mailData.AttachmentFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            mailData.AttachmentFile.CopyTo(memoryStream);
                            emailBodyBuilder.Attachments.Add(mailData.AttachmentFile.FileName, memoryStream.ToArray(), ContentType.Parse(mailData.AttachmentFile.ContentType));
                        }
                    }

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    if (mailData.MessageID != null)
                    {
                        emailMessage.Headers.Add("Message-ID", mailData.MessageID);
                    }
                    if (mailData.MessageReplyToID != null)
                    {
                        emailMessage.Headers.Add("In-Reply-To", mailData.MessageReplyToID);
                    }
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }
               
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
