using TourTravel.Models;

namespace TourTravel.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
