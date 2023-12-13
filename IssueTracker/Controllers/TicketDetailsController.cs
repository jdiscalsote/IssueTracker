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
                Description = ticketDetails.Tables[0].Rows[0]["Description"].ToString(),
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
                CreatedDate = Convert.ToDateTime(ticketDetails.Tables[0].Rows[0]["CreatedDate"]).ToString("MMMM dd, yyy hh:mm:ss tt"),
                CreatedBy = ticketDetails.Tables[0].Rows[0]["CreatedBy"].ToString(),
            };

            return detailsObj;
        }
    }
}
