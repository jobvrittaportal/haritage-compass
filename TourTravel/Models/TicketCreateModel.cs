namespace TourTravel.Models
{
  public class TicketCreateModel
  {
    public int Application_Group_Id { get; set; }
    public int Ticket_Number { get; set; }
    public int Ticket_Id { get; set; }
    public int Assigned_Group_Id { get; set; }
    public string Priority { get; set; }
    public string Impact { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }

    public IFormFile? Screenshot { get; set; }


  }


}
