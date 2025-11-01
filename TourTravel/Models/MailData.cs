using System.Collections;

namespace TourTravel.Models
{
    public class MailData
    {
        public string? SenderName { get; set; }
        public string? SenderEmail { get; set; }
        public string[] EmailToId { get; set; }
        public ArrayList? EmailToCC { get; set; }
        public ArrayList? EmailToBCC { get; set; }
        public string[] Attachments { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string? MessageID { get; set; }
        public string? MessageReplyToID { get; set; }
        public IFormFile? AttachmentFile { get; set; }
    }
}
