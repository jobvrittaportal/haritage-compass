using System.Collections.Generic;
using System.Threading.Tasks;
using TourTravel.Models;

public interface ISupportService
{
  Task<List<ApplicationModel>> GetApplications();
  Task<List<AssignGroupModel>> GetAssignGroups();
  Task<bool> CreateTicket(TicketCreateModel model);
}

public class ApplicationModel
{
  public int Id { get; set; }
  public string ApplicationName { get; set; }
}

public class AssignGroupModel
{
  public int Id { get; set; }
  public string GroupName { get; set; }
}
