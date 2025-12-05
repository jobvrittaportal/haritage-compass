namespace TourTravel.Models
{
  public class TicketHistoryEntry
  {
    public int historyId { get; set; }
    public string updatedBy { get; set; }
    public string updatedAt { get; set; }
    public List<TicketHistoryChange> changes { get; set; }
  }

  public class TicketHistoryChange
  {
    public string field { get; set; }
    public string oldValue { get; set; }
    public string newValue { get; set; }
  }

}
