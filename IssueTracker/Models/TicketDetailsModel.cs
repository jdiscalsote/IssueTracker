using System.ComponentModel;

namespace IssueTracker.Models
{
    public class TicketDetailsModel
    {
        public string TicketID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string InternalNote { get; set; }
        public string Category { get; set; }
        public string RequestType { get; set; }
        public string Priority { get; set; }
        public string Assigned_QMS { get; set; }
        public string Assigned_Programmer { get; set; }
        public string Requester { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
