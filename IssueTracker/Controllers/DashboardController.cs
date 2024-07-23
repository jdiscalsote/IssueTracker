using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using IssueTracker.Models;
using IssueTracker.SystemModels;
using IssueTracker.SystemServices;

namespace IssueTracker.Controllers
{
    public class DashboardController : Controller
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

        public DashboardController(GeneralServices _requestServices)
        {
            requestServices = _requestServices;
        }

        public IActionResult Dashboard()
        {
            var accessCode = HttpContext.Session.GetString("AccessCode");
            int? roleId = HttpContext.Session.GetInt32("RoleId");

            if (roleId.HasValue)
            {
                // Call the GetRoleName method
                DataSet dsRoleName = requestServices.GetUserRoleName(roleId.Value, "getRoleName");
                if (dsRoleName != null && dsRoleName.Tables.Count > 0 && dsRoleName.Tables[0].Rows.Count > 0)
                {
                    string roleName = dsRoleName.Tables[0].Rows[0]["RoleName"].ToString();
                    ViewBag.RoleName = roleName;
                }

                // Call the GetUsername method
                DataSet dsUsername = requestServices.GetUsername(accessCode, "getUsername");
                if (dsUsername != null && dsUsername.Tables.Count > 0 && dsUsername.Tables[0].Rows.Count > 0)
                {
                    string userName = dsUsername.Tables[0].Rows[0]["UserName"].ToString();
                    ViewBag.UserName = userName;
                }
            }

            var ticketData = new DashboardModel();

            DataSet getChart = requestServices.GetPriorityCounts("Chart");

            List<DataPoint> dataPoints = new();

            foreach (DataRow row in getChart.Tables[0].Rows)
            {
                dataPoints.Add(new DataPoint(row["PriorityName"].ToString(), Convert.ToInt32(row["Count"])));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View(GetListPage(ticketData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Dashboard([FromForm] DashboardModel dashModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Alert = AlertServices.ShowAlert(Alerts.Warning, "Invalid data. Please correct the errors and try again.");
                return View("Dashboard", dashModel);
            }

            return View(GetListPage(dashModel));
        }

        private DashboardModel GetListPage(DashboardModel dashCD)
        {
            if (string.IsNullOrEmpty(dashCD.SelectedValue)) //Get open tickets list (Default: Onload) 
            {
                var accessCode = HttpContext.Session.GetString("AccessCode");
                int roleId = (int)HttpContext.Session.GetInt32("RoleId");

                DataSet tableList = requestServices.GetOpenTickets(accessCode, roleId);
                DashboardModel dashModelList = new();

                if (tableList.Tables.Count > 0 && tableList.Tables[0].Rows.Count > 0)
                {
                    dashModelList.GetOpenTicketTable = new List<DashboardModelObject>();
                    foreach (DataRow row in tableList.Tables[0].Rows)
                    {
                        dashModelList.GetOpenTicketTable.Add(new DashboardModelObject
                        {
                            TicketID = row["TicketID"].ToString(),
                            Subject = row["Subject"].ToString(),
                            Category = row["Category"].ToString(),
                            Status = row["Status"].ToString(),
                            Priority = row["Priority"].ToString()
                        });
                    }

                    dashModelList.GroupButtonValues = GetgroupButtonValue();
                    dashModelList.GetDashTicketsTitle = GetDashTicketsTitle();                    

                    return dashModelList;
                }
                else { return null; }
            }
            else
            {
                string accessCode = HttpContext.Session.GetString("AccessCode");
                int roleId = (int)HttpContext.Session.GetInt32("RoleId");

                DataSet tableList = requestServices.GetOpenTickets(accessCode, roleId);
                DashboardModel dashModelList = new();

                if (tableList.Tables.Count > 0 && tableList.Tables[0].Rows.Count > 0)
                {
                    dashModelList.GetOpenTicketTable = new List<DashboardModelObject>();
                    foreach (DataRow row in tableList.Tables[0].Rows)
                    {
                        dashModelList.GetOpenTicketTable.Add(new DashboardModelObject
                        {
                            TicketID = row["TicketID"].ToString(),
                            Subject = row["Subject"].ToString(),
                            Category = row["Category"].ToString(),
                            Status = row["Status"].ToString(),
                            Priority = row["Priority"].ToString()
                        });
                    }

                    dashModelList.GroupButtonValues = GetgroupButtonValue();
                    dashModelList.GetDashTicketsTitle = GetDashTicketsTitle();

                    return dashModelList;
                }
                else { return null; }
            }
        }

        private List<GroupButtonValue> GetgroupButtonValue()
        {
            var getConn = GetConnection()
                    .GetSection("ConnectionStrings")
                    .GetSection("MSSQLSERVER2023").Value;

            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("Select paramcode, paramsubname " +
                                "From dbo.ParameterSub " +
                                "Where ParamID = 5 and Status = 1 " +
                                "Order By paramcode", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<GroupButtonValue> groupList = new();
            if (odr.HasRows)
            {
                while (odr.Read())
                {
                    groupList.Add(new GroupButtonValue
                    {
                        Id = Convert.ToInt32(odr["paramcode"]),
                        Value = Convert.ToString(odr["paramsubname"])
                    });
                }
            }
            conn.Close();
            return groupList;
        }

        private List<DashTicketsTitle> GetDashTicketsTitle()
        {
            var getConn = GetConnection()
                    .GetSection("ConnectionStrings")
                    .GetSection("MSSQLSERVER2023").Value;

            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("Select paramcode, " +
                                "(Select Count(status) From dbo.Tickets Where status = ParamCode) as count, paramsubname " +
                                "From dbo.ParameterSub " +
                                "Where ParamID = 4 and Status = 1 " +
                                "Order By paramcode", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<DashTicketsTitle> groupList = new();
            if (odr.HasRows)
            {
                while (odr.Read())
                {
                    groupList.Add(new DashTicketsTitle
                    {
                        ID = Convert.ToInt32(odr["paramcode"]),
                        Count = Convert.ToInt32(odr["count"]),
                        Title = Convert.ToString(odr["paramsubname"])
                    });
                }
            }
            conn.Close();
            return groupList; 
        }
    }
}
