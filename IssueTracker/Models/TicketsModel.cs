using System.ComponentModel;

namespace IssueTracker.Models
{
    public class TicketsModelObject
    {
        public string TotalRecords { get; set; }
        public string SelectedValue { get; set; }

        [DisplayName("Ticket ID")]
        public string TicketID { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        [DisplayName("Request Type")]
        public string RequestType { get; set; }
        public string Priority { get; set; }
        public string Requester { get; set; }
        public string Status { get; set; }
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }
    }

    public class TicketsModel : TicketsModelObject
    {
        public List<TicketsModelObject> TicketList { get; set; }
        public List<ButtonValue> Button_Value { get; set; } 
    }

    public class ButtonValue
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
