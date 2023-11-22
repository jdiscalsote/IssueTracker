using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using IssueTracker.SystemServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTracker.Models
{
    public class TicketsModelObject
    {
        public string Search { get; set; }
        public string TotalRecords { get; set; }
        public string CurrentPage { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
        public string SelectedValue { get; set; }

        [DisplayName("Ticket ID")]
        public string TicketID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; } 
        public string InternalNote { get; set; }
        public string Category { get; set; }
        public string RequestType { get; set; }
        public string Priority { get; set; }

        [DisplayName("Assigned QMS")]
        public string Assigned_QMS { get; set; }

        [DisplayName("Assigned Programmer")]
        public string Assigned_Programmer { get; set; }
        public string Requester { get; set; }
        public string Status { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
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
