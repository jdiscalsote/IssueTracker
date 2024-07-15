using System.Data;
using Microsoft.AspNetCore.Mvc;

using IssueTracker.SystemServices;
using IssueTracker.Models;

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

            var accessCode = HttpContext.Session.GetString("AccessCode");

            return View(SettingsInfo(accessCode));
        }

        public AccountModel SettingsInfo(string accessCode)
        {
            if (ModelState.IsValid)
            {
                DataSet accountInfo = requestServices.GetAccountSettings(accessCode, "AccountInfo");

                if (accountInfo != null && accountInfo.Tables.Count > 0 && accountInfo.Tables[0].Rows.Count > 0)
                {
                    AccountModel detailsObj = new()
                    {
                        EmployeeNo = accountInfo.Tables[0].Rows[0]["EmpNo"].ToString(),
                        FirstName = accountInfo.Tables[0].Rows[0]["FNam"].ToString(),
                        MiddleName = accountInfo.Tables[0].Rows[0]["MNam"].ToString(),
                        LastName = accountInfo.Tables[0].Rows[0]["LNam"].ToString(),
                        Role = accountInfo.Tables[0].Rows[0]["Role"].ToString(),
                        Email = accountInfo.Tables[0].Rows[0]["Email"].ToString(),
                        Department = accountInfo.Tables[0].Rows[0]["Department"].ToString(),
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
