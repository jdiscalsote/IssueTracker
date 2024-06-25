using System.Data;
using Microsoft.AspNetCore.Mvc;

using IssueTracker.Models;
using IssueTracker.SystemServices;

namespace IssueTracker.Controllers
{
    public class TicketDetailsController : Controller
    {
        readonly GeneralServices requestServices;

        //Set SQL DB Connection
        public IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            return builder;
        }

        public TicketDetailsController(GeneralServices _requestServices)
        {
            requestServices = _requestServices;
        }

        public IActionResult TicketDetails()
        {
            int? roleId = HttpContext.Session.GetInt32("RoleId");

            if (roleId.HasValue)
            {
                // Call the GetRoleName method
                DataSet dsRoleName = GetRoleName(roleId.Value);
                if (dsRoleName != null && dsRoleName.Tables.Count > 0 && dsRoleName.Tables[0].Rows.Count > 0)
                {
                    string roleName = dsRoleName.Tables[0].Rows[0]["RoleName"].ToString();
                    ViewBag.RoleName = roleName;
                }
            }

            var ticketId = HttpContext.Request.RouteValues["ID"];
            ViewBag.TicketID = "#" + ticketId;

            return View(Ticket(ticketId.ToString()));
        }

        public TicketDetailsModel Ticket(string ticketId)
        {
            if (ModelState.IsValid)
            {
                DataSet ticketDetails = requestServices.GetTicketDetails(ticketId);

                if (ticketDetails != null && ticketDetails.Tables.Count > 0 && ticketDetails.Tables[0].Rows.Count > 0)
                {
                    TicketDetailsModel detailsObj = new()
                    {
                        Subject = ticketDetails.Tables[0].Rows[0]["Subject"].ToString(),
                        Description = ticketDetails.Tables[0].Rows[0]["Description"].ToString(),
                        InternalNote = ticketDetails.Tables[0].Rows[0]["InternalNote"].ToString(),
                        Category = ticketDetails.Tables[0].Rows[0]["Category"].ToString(),
                        RequestType = ticketDetails.Tables[0].Rows[0]["RequestType"].ToString(),
                        Priority = ticketDetails.Tables[0].Rows[0]["Priority"].ToString(),
                        Status = ticketDetails.Tables[0].Rows[0]["Status"].ToString(),
                        Requester = ticketDetails.Tables[0].Rows[0]["Requester"].ToString(),
                        Organization = ticketDetails.Tables[0].Rows[0]["Organization"].ToString(),
                        Email = ticketDetails.Tables[0].Rows[0]["Email"].ToString(),
                        Role = ticketDetails.Tables[0].Rows[0]["Role"].ToString(),
                        Assigned_QMS = ticketDetails.Tables[0].Rows[0]["Assigned_QMS"].ToString(),
                        Assigned_Programmer = ticketDetails.Tables[0].Rows[0]["Assigned_Programmer"].ToString(),
                        CreatedDate = Convert.ToDateTime(ticketDetails.Tables[0].Rows[0]["CreatedDate"]).ToString("MMMM dd, yyyy hh:mm:ss tt"),
                        CreatedBy = ticketDetails.Tables[0].Rows[0]["CreatedBy"].ToString(),
                    };

                    return detailsObj;
                }
            }

            return null;
        }


        public DataSet GetRoleName(int roleId)
        {
            if (ModelState.IsValid)
            {
                DataSet dsRoleName = requestServices.GetRoleName(roleId);

                if (dsRoleName != null && dsRoleName.Tables.Count > 0 && dsRoleName.Tables[0].Rows.Count > 0)
                {
                    string roleName = dsRoleName.Tables[0].Rows[0]["RoleName"].ToString();
                    TempData["RoleName"] = roleName;
                    return dsRoleName;
                }

                return null;
            }

            return null;
        }
    }
}
