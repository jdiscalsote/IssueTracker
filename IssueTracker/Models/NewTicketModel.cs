using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IssueTracker.Models
{
    public class NewTicketModelObject
    {
        public string TicketID { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string InternalNote { get; set; }
        public int Category { get; set; }
        public int RequestType { get; set; }
        public int Priority { get; set; }
        public string Assigned_QMS { get; set; }
        public string Assigned_Programmer { get; set; }
        public string Requester { get; set; }
        public string UserID { get; set; }
        public string Organization_Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }    
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

    }

    public class NewTicketModel : NewTicketModelObject
    {
        public List<CategoryList> Category_List { get; set; }
        public List<RequestList> Request_List { get; set; }
        public List<RequestPriorityList> RequestPriority_List { get; set; }
        public List<RequesterList> Requester_List { get; set; }
        public List<AssigneeQmsList> AssigneeQMS_List { get; set; }
        public List<AssigneeProgList> AssigneeProg_List { get; set; }
    }

    public class CategoryList
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
    }

    public class RequestList
    {
        public string RequestCode { get; set; }
        public string RequestName { get; set; }
    }

    public class RequestPriorityList
    {
        public string PriorityCode { get; set; }
        public string PriorityName { get; set; }
    }

    public class RequesterList
    {
        public string UserID { get; set; }
        public string Username { get; set; }
    }

    public class AssigneeQmsList
    {
        public string UserID { get; set; }
        public string Username { get; set; }
    }

    public class AssigneeProgList
    {
        public string UserID { get; set; }
        public string Username { get; set; }
    }
}
