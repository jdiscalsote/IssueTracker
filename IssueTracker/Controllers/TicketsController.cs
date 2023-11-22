using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using IssueTracker.Models;
using IssueTracker.SystemServices;

namespace IssueTracker.Controllers
{
    public class TicketsController : Controller
    {
        readonly GeneralServices requestServices;

        //Set SQL DB Connection
        public IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            return builder;
        }

        public TicketsController(GeneralServices _requestServices)
        {
            requestServices = _requestServices;
        }

        public IActionResult Tickets()
        {
            TicketsModel ticketsModel = new();

            return View(GetListPage(ticketsModel));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Tickets([FromForm] TicketsModel ticketsModel)
        {
            return View(GetListPage(ticketsModel));
        }

        private TicketsModel GetListPage(TicketsModel priorityCD)
        {
            if (string.IsNullOrEmpty(priorityCD.SelectedValue)) //Get all request list (Default: Onload) 
            {
                var value = HttpContext.Session.GetString("AccessCode");
                DataSet tableList = requestServices.GetTicketsbyCreatedBy(value);
                TicketsModel ticketsModel = new();

                if (tableList.Tables.Count > 0 && tableList.Tables[0].Rows.Count > 0)
                {
                    ticketsModel.TicketList = new List<TicketsModelObject>();
                    foreach (DataRow row in tableList.Tables[0].Rows)
                    {
                        ticketsModel.TicketList.Add(new TicketsModelObject
                        {
                            TicketID = row["TicketID"].ToString(),
                            Subject = row["Subject"].ToString(),
                            Category = row["Category"].ToString(),
                            Status = row["Status"].ToString(),
                            Priority = row["Priority"].ToString(),
                            Requester = row["Requester"].ToString(),
                            Assigned_QMS = row["Assigned_QMS"].ToString(),
                            CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                        });

                        ticketsModel.TotalRecords = "1 to 10 of " + tableList.Tables[0].Rows.Count.ToString() + " records";
                    }

                    ticketsModel.Button_Value = GetButtonValue();

                    return ticketsModel;
                }
                else { return null; }
            }
            else 
            {
                var value = HttpContext.Session.GetString("AccessCode");
                DataSet tableList = requestServices.GetTicketsbyCreatedBy(value);
                TicketsModel ticketsModel = new();

                if (tableList.Tables.Count > 0 && tableList.Tables[0].Rows.Count > 0)
                {
                    ticketsModel.TicketList = new List<TicketsModelObject>();
                    foreach (DataRow row in tableList.Tables[0].Rows)
                    {
                        ticketsModel.TicketList.Add(new TicketsModelObject
                        {
                            TicketID = row["TicketID"].ToString(),
                            Subject = row["Subject"].ToString(),
                            Category = row["Category"].ToString(),
                            Status = row["Status"].ToString(),
                            Priority = row["Priority"].ToString(),
                            Requester = row["Requester"].ToString(),
                            CreatedBy = row["CreatedBy"].ToString(),
                            Assigned_QMS = row["Assigned_QMS"].ToString()
                        });
                    }

                    ticketsModel.Button_Value = GetButtonValue();

                    return ticketsModel;
                }
                else { return null; }
            }
        }

        private List<ButtonValue> GetButtonValue()
        {
            var getConn = GetConnection().GetSection("ConnectionStrings").GetSection("MSSQLSERVER2023").Value;
            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("select paramcode, paramsubname from dbo.ParameterSub where ParamID = 3 and Status = 1 order by paramcode", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<ButtonValue> prioList = new List<ButtonValue>();
            if (odr.HasRows)
            {
                while (odr.Read())
                {
                    prioList.Add(new ButtonValue
                    {
                        Id = Convert.ToInt32(odr["paramcode"]),
                        Value = Convert.ToString(odr["paramsubname"])
                    });
                }
            }
            conn.Close();
            return prioList;
        }
    }
}
