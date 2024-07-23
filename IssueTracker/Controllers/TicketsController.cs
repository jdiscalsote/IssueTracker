using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using IssueTracker.Models;
using IssueTracker.SystemModels;
using IssueTracker.SystemServices;

namespace IssueTracker.Controllers
{
    public class TicketsController : Controller
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

        public TicketsController(GeneralServices _requestServices)
        {
            requestServices = _requestServices;
        }

        public IActionResult Tickets()
        {
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
            }

            TicketsModel ticketsModel = new();

            return View(GetListPage(ticketsModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Tickets([FromForm] TicketsModel ticketsModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Alert = AlertServices.ShowAlert(Alerts.Warning, "Invalid data. Please correct the errors and try again.");
                return View("Tickets", ticketsModel);
            }

            return View(GetListPage(ticketsModel));
        }

        private TicketsModel GetListPage(TicketsModel ticketsModel)
        {
            if (string.IsNullOrEmpty(ticketsModel.SelectedValue))
            {
                DataSet tableList = requestServices.GetTicketList();
                TicketsModel resultModel = new();

                if (tableList.Tables.Count > 0 && tableList.Tables[0].Rows.Count > 0)
                {
                    resultModel.TicketList = new List<TicketsModelObject>();
                    foreach (DataRow row in tableList.Tables[0].Rows)
                    {
                        resultModel.TicketList.Add(new TicketsModelObject
                        {
                            TicketID = row["TicketID"].ToString(),
                            Subject = row["Subject"].ToString(),
                            Category = row["Category"].ToString(),
                            RequestType = row["RequestType"].ToString(),
                            Priority = row["Priority"].ToString(),
                            Status = row["Status"].ToString(),
                            Requester = row["Requester"].ToString(),
                            CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                        });
                    }

                    resultModel.TotalRecords = $"1 to 10 of {tableList.Tables[0].Rows.Count} records";
                    resultModel.Button_Value = GetButtonValue();

                    return resultModel;
                }
                else 
                { 
                    return null; 
                }
            }
            else 
            {
                return null;
            }
        }

        private List<ButtonValue> GetButtonValue()
        {
            var getConn = GetConnection()
                    .GetSection("ConnectionStrings")
                    .GetSection("MSSQLSERVER2023").Value;
            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("Select paramcode, paramsubname " +
                                "From dbo.ParameterSub " +
                                "Where ParamID = 3 and Status = 1 " +
                                "Order By paramcode", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<ButtonValue> prioList = new();
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
