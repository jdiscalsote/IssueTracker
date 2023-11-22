using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using IssueTracker.Models;
using IssueTracker.SystemServices;

namespace IssueTracker.Controllers
{
    public class DashboardController : Controller
    {
        readonly GeneralServices requestServices;

        //Set SQL DB Connection
        public IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            return builder;
        }

        public DashboardController(GeneralServices _requestServices)
        {
            requestServices = _requestServices;
        }

        public IActionResult Dashboard()
        {
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
            return View(GetListPage(dashModel));
        }

        private DashboardModel GetListPage(DashboardModel dashCD)
        {
            if (string.IsNullOrEmpty(dashCD.SelectedValue)) //Get open tickets list (Default: Onload) 
            {
                var value = HttpContext.Session.GetString("AccessCode");
                DataSet tableList = requestServices.GetOpenTickets(value);
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
                string value = HttpContext.Session.GetString("AccessCode");
                DataSet tableList = requestServices.GetOpenTickets(value);
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
            var getConn = GetConnection().GetSection("ConnectionStrings").GetSection("MSSQLSERVER2023").Value;
            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("select paramcode, paramsubname from dbo.ParameterSub where ParamID = 5 and Status = 1 order by paramcode", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<GroupButtonValue> groupList = new List<GroupButtonValue>();
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
            var getConn = GetConnection().GetSection("ConnectionStrings").GetSection("MSSQLSERVER2023").Value;
            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("select paramcode, (select count(status) from dbo.Tickets where status = ParamCode) as count, paramsubname from dbo.ParameterSub where ParamID = 4 and Status = 1 order by paramcode", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<DashTicketsTitle> groupList = new List<DashTicketsTitle>();
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
