namespace TourTravel.Models.Support
{
  public class CreateTicketModel
  {
    public string subject { get; set; }
    public string description { get; set; }
    public int priority { get; set; }
    public IFormFile? attachment { get; set; }
  }

  public class TicketListItem
  {
    public int ticketId { get; set; }
    public string ticket_Number { get; set; }
    public string subject { get; set; }
    public string createdBy { get; set; }
    public string createdAt { get; set; }
    public string status { get; set; }
  }

  public class TicketDetail
  {
    public int ticketId { get; set; }
    public string ticket_Number { get; set; }
    public string subject { get; set; }
    public string description { get; set; }
    public string priority { get; set; }
    public string createdBy { get; set; }
    public string createdAt { get; set; }
    public string status { get; set; }
    public string attachmentURL { get; set; }
  }

  public class TicketHistoryItem
  {
    public int historyId { get; set; }
    public string updatedBy { get; set; }
    public string updatedAt { get; set; }
    public List<TicketChange> changes { get; set; }
  }

  public class TicketChange
  {
    public string field { get; set; }
    public string oldValue { get; set; }
    public string newValue { get; set; }
  }
}
