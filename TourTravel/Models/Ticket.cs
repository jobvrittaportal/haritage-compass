namespace TourTravel.Models
{
  public class Ticket
  {
    public int ticket_Number { get; set; }
    public string ticket_Id { get; set; }
    public int application_Group_Id { get; set; }
    public string priority { get; set; }
    public string impact { get; set; }
    public int assigned_Group_Id { get; set; }
    public string subject { get; set; }
    public string description { get; set; }
    public int status { get; set; }
    public string application_Group_Name { get; set; }
    public string assigned_Grouped_Name { get; set; }
    public string status_Name { get; set; }
    public string open { get; set; }
    public int raised_By { get; set; }
    public string raised_By_Name { get; set; }
    public int assign_To { get; set; }
    public string agent_Name { get; set; }
    public string file { get; set; }
  }

  public class TicketResponse
  {
    public int totalCount { get; set; }
    public List<Ticket> result { get; set; }
  }
}
