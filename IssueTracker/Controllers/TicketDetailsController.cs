using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using IssueTracker.Models;
using IssueTracker.SystemModels;
using IssueTracker.SystemServices;
using Azure.Core;

namespace IssueTracker.Controllers
{
    public class TicketDetailsController : Controller
    {
        readonly GeneralServices requestServices;

        //Set SQL DB Connection
        public IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            return builder;
        }

        public TicketDetailsController(GeneralServices _requestServices)
        {
            requestServices = _requestServices;
        }

        public IActionResult TicketDetails()
        {
            var ticketId = HttpContext.Request.RouteValues["ID"];
            ViewBag.TicketID = "#" + ticketId;

            var value = HttpContext.Session.GetString("AccessCode");

            return View(Ticket(ticketId.ToString(), value));
        }

        public TicketDetailsModel Ticket(string _ticketID, string _accessCode)
        {
            DataSet ticketDetails = requestServices.GetTicketDetails(_ticketID, _accessCode);

            TicketDetailsModel detailsObj = new()
            {
                Subject = ticketDetails.Tables[0].Rows[0]["Subject"].ToString(),
                Category = ticketDetails.Tables[0].Rows[0]["Category"].ToString(),
                Status = ticketDetails.Tables[0].Rows[0]["Status"].ToString(),
                Priority = ticketDetails.Tables[0].Rows[0]["Priority"].ToString(),
                Requester = ticketDetails.Tables[0].Rows[0]["Requester"].ToString(),
                Assigned_QMS = ticketDetails.Tables[0].Rows[0]["Assigned_QMS"].ToString(),
                CreatedDate = Convert.ToDateTime(ticketDetails.Tables[0].Rows[0]["CreatedDate"]).ToString("MMMM dd, yyy hh:mm:ss tt"),
                CreatedBy = ticketDetails.Tables[0].Rows[0]["CreatedBy"].ToString(),
            };

            return detailsObj;
        }
    }
}
