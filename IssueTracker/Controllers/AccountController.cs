using System.Data;
using Microsoft.AspNetCore.Mvc;

using IssueTracker.Models;
using IssueTracker.SystemServices;

namespace IssueTracker.Controllers
{
    public class AccountController : Controller
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

        public AccountController(GeneralServices _requestServices)
        {
            requestServices = _requestServices;
        }

        public IActionResult Account()
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

            return View();
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
