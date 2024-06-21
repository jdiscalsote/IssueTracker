using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

using IssueTracker.Models;
using IssueTracker.SystemModels;
using IssueTracker.SystemServices;

namespace IssueTracker.Controllers
{
    public class LoginController : Controller
    {
        //Set SQL DB Connection
        public IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            return builder;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Validate([FromForm] LoginModel collection)
        {
            string userNam = collection.AccessCode;
            string userPass = collection.Password;

            if (string.IsNullOrEmpty(userNam) && string.IsNullOrEmpty(userPass))
            {
                ViewBag.Alert = AlertServices.ShowAlert(Alerts.Warning, "Input username and password");
                return PartialView("Index");
            }

            if (string.IsNullOrEmpty(userNam))
            {
                ViewBag.Alert = AlertServices.ShowAlert(Alerts.Warning, "Please input your username.");
                return PartialView("Index");
            }

            if (string.IsNullOrEmpty(userPass))
            {
                ViewBag.Alert = AlertServices.ShowAlert(Alerts.Warning, "Please input your password.");
                return PartialView("Index");
            }

            try
            {
                var connectionString = GetConnection().GetSection("ConnectionStrings").GetSection("MSSQLSERVER2023").Value;

                using (SqlConnection conn = new(connectionString))
                using (SqlCommand cmd = new("sp_UserLogin", conn))
                {
                    conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@strAccessCode", SqlDbType.NVarChar, 255) { Value = userNam });
                    cmd.Parameters.Add(new SqlParameter("@strPassword", SqlDbType.NVarChar, 255) { Value = userPass });

                    SqlParameter roleIdParam = new("@RoleId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(roleIdParam);

                    SqlParameter statusParam = new("@status", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(statusParam);

                    cmd.ExecuteNonQuery();

                    int status = (int)cmd.Parameters["@status"].Value;
                    int? roleId = cmd.Parameters["@RoleId"].Value as int?;

                    HttpContext.Session.SetString("AccessCode", userNam);

                    ViewBag.Status = status;

                    if (status == 1)
                    {
                        ViewBag.Alert = AlertServices.ShowAlert(Alerts.Success, "Login Success!");

                        if (roleId.HasValue)
                        {
                            HttpContext.Session.SetInt32("RoleId", roleId.Value);
                        }
                    }
                    else
                    {
                        ViewBag.Alert = AlertServices.ShowAlert(Alerts.Danger, "Invalid account, please try again.");
                    }

                    ModelState.Clear();
                    conn.Close();
                }

                return PartialView("Index");
            }
            catch (SqlException)
            {
                // Handle specific SQL exceptions here
                ViewBag.Alert = AlertServices.ShowAlert(Alerts.Danger, "An error occurred. Please try again later.");
                return PartialView("Index");
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                ViewBag.Alert = AlertServices.ShowAlert(Alerts.Danger, "An error occurred: " + ex.Message);
                return PartialView("Index");
            }
        }
    }
}
