using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace IssueTracker.Models
{
    public class DashboardModelObject
    {
        public string SelectedValue { get; set; }

        [DisplayName("Ticket ID")]
        public string TicketID { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
    }

    public class DashboardModel : DashboardModelObject
    {
        public List<GroupButtonValue> GroupButtonValues { get; set; }
        public List<DashTicketsTitle> GetDashTicketsTitle { get; set; }
        public List<DashboardModelObject> GetOpenTicketTable { get; set; }  
    }

    public class GroupButtonValue
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class DashTicketsTitle
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
    }

    [DataContract]
    public class DataPoint
    {
        public DataPoint(string label, int y)
        {
	    	this.Label = label;
            this.Y = y;
        }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "y")]

        public int Y { get; set; }

    }
}
