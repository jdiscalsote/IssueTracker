using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

using IssueTracker.Models;
using IssueTracker.SystemModels;
using IssueTracker.SystemServices;

namespace IssueTracker.Controllers
{
    public class NewTicketController : Controller
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

        public NewTicketController(GeneralServices _requestServices)
        {
            requestServices = _requestServices;
        }

        public IActionResult NewTicket()
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

            return View(GetListofValue());
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

        private List<CategoryList> GetCategoryList()
        {
            var getConn = GetConnection()
                    .GetSection("ConnectionStrings")
                    .GetSection("MSSQLSERVER2023").Value;

            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("Select paramcode, paramsubname " +
                                "From dbo.ParameterSub " +
                                "Where ParamID = 1 and Status = 1", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<CategoryList> categList = new();
            if (odr.HasRows)
            {
                while (odr.Read())
                {
                    categList.Add(new CategoryList
                    {
                        CategoryCode = Convert.ToString(odr["paramcode"]),
                        CategoryName = Convert.ToString(odr["paramsubname"])
                    });
                }
            }
            conn.Close();
            return categList;
        }

        private List<RequestList> GetRequestList()
        {
            var getConn = GetConnection()
                .GetSection("ConnectionStrings")
                .GetSection("MSSQLSERVER2023").Value;

            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("Select paramcode, paramsubname From dbo.ParameterSub " +
                                "Where ParamID = 2 and Status = 1", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<RequestList> reqList = new();
            if (odr.HasRows)
            {
                while (odr.Read())
                {
                    reqList.Add(new RequestList
                    {
                        RequestCode = Convert.ToString(odr["paramcode"]),
                        RequestName = Convert.ToString(odr["paramsubname"])
                    });
                }
            }
            conn.Close();
            return reqList;
        }

        private List<RequestPriorityList> GetRequestPriorityList()
        {
            var getConn = GetConnection()
                    .GetSection("ConnectionStrings")
                    .GetSection("MSSQLSERVER2023").Value;

            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("Select paramcode, paramsubname " +
                                "From dbo.ParameterSub " +
                                "Where ParamID = 3 and Status = 1", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<RequestPriorityList> prioList = new();
            if (odr.HasRows)
            {
                while (odr.Read())
                {
                    prioList.Add(new RequestPriorityList
                    {
                        PriorityCode = Convert.ToString(odr["paramcode"]),
                        PriorityName = Convert.ToString(odr["paramsubname"])
                    });
                }
            }
            conn.Close();
            return prioList;
        }

        private List<RequesterList> GetRequesterList()
        {
            var getConn = GetConnection()
                    .GetSection("ConnectionStrings")
                    .GetSection("MSSQLSERVER2023").Value;

            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("Select USER_ID, CONCAT(LAST_NAME,', ',FIRST_NAME,' ',MIDDLE_NAME) as Username, ROLE_ID " +
                                "From dbo.UserProfile", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<RequesterList> reqst = new();
            if (odr.HasRows)
            {
                while (odr.Read())
                {
                    reqst.Add(new RequesterList
                    {
                        UserID = Convert.ToString(odr["USER_ID"]),
                        Username = Convert.ToString(odr["Username"])
                    });
                }
            }
            conn.Close();
            return reqst;
        }

        private List<AssigneeQmsList> GetAssigneeQMSList()
        {
            var getConn = GetConnection()
                    .GetSection("ConnectionStrings")
                    .GetSection("MSSQLSERVER2023").Value;

            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("Select USER_ID, CONCAT(LAST_NAME,', ',FIRST_NAME,' ',MIDDLE_NAME) as Username " +
                                "From dbo.UserProfile", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<AssigneeQmsList> qms = new();
            if (odr.HasRows)
            {
                while (odr.Read())
                {
                    qms.Add(new AssigneeQmsList
                    {
                        UserID = Convert.ToString(odr["USER_ID"]),
                        Username = Convert.ToString(odr["Username"])
                    });
                }
            }
            conn.Close();
            return qms;
        }

        private List<AssigneeProgList> GetAssigneeProgList()
        {
            var getConn = GetConnection()
                    .GetSection("ConnectionStrings")
                    .GetSection("MSSQLSERVER2023").Value;

            SqlConnection conn = new(getConn);
            SqlCommand cmd = new("Select USER_ID, CONCAT(LAST_NAME,', ',FIRST_NAME,' ',MIDDLE_NAME) as Username " +
                                "From dbo.UserProfile", conn);
            conn.Open();
            SqlDataReader odr = cmd.ExecuteReader();
            List<AssigneeProgList> prog = new();
            if (odr.HasRows)
            {
                while (odr.Read())
                {
                    prog.Add(new AssigneeProgList
                    {
                        UserID = Convert.ToString(odr["USER_ID"]),
                        Username = Convert.ToString(odr["Username"])
                    });
                }
            }
            conn.Close();
            return prog;
        }

        private NewTicketModel GetListofValue()
        {
            NewTicketModel model = new()
            {
                Category_List = GetCategoryList(),
                Request_List = GetRequestList(),
                RequestPriority_List = GetRequestPriorityList(),
                Requester_List = GetRequesterList(),
                AssigneeQMS_List = GetAssigneeQMSList(),
                AssigneeProg_List = GetAssigneeProgList()
            };

            return model;
        }

        [HttpGet]
        [Route("/NewTicket/GetRequesterDetails")]
        public IActionResult GetRequesterDetails(string user_id)
        {
            if (ModelState.IsValid)
            {
                // Fetch user details from a database based on user_id
                var userDetails = FetchUserDetailsFromDatabase(user_id);

                // Check if userDetails has data before accessing it
                if (userDetails != null && userDetails.Tables.Count > 0 && userDetails.Tables[0].Rows.Count > 0)
                {
                    var userID = userDetails.Tables[0].Rows[0]["USER_ID"].ToString();
                    var deptDesc = userDetails.Tables[0].Rows[0]["DEPARTMENT_DESC"].ToString();
                    var roleID = userDetails.Tables[0].Rows[0]["ROLE_ID"].ToString();
                    var email = userDetails.Tables[0].Rows[0]["EMAIL"].ToString();

                    // Return the user details as JSON
                    return Ok(new { UserID = userID, DepartmentDesc = deptDesc, RoleID = roleID, Email = email });
                }
                else
                {
                    return NotFound(); // Return a 404 Not Found if user details are not found
                }
            }
            return NotFound();
        }

        public DataSet FetchUserDetailsFromDatabase(string user_id)
        {
            var dataSet = new DataSet();

            if (ModelState.IsValid)
            {
                var getConn = GetConnection()
                .GetSection("ConnectionStrings")
                .GetSection("MSSQLSERVER2023").Value;

                using SqlConnection conn = new(getConn);

                SqlCommand cmd = new("Select USER_ID, d.DEPARTMENT_DESC, ROLE_ID, EMAIL " +
                                    "From dbo.UserProfile u " +
                                    "Left Join dbo.Department d on u.DEPARTMENT_ID = d.DEPT_ID " +
                                    "Where u.USER_ID = @user_id", conn);

                cmd.Parameters.AddWithValue("@user_id", user_id);

                try
                {
                    conn.Open();
                    using var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataSet);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }

                return dataSet;
            }
            return dataSet;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewTicket([FromForm] NewTicketModel newObj)
        {
            SequenceServices sequenceServices = new();
            SequenceModel sequence = sequenceServices.SequenceCodeGenerate("Ticket_ID");
            var ticketID = sequence.NewSequence;

            if (ModelState.IsValid)
            {
                newObj.TicketID = ticketID;
                newObj.Requester = ModelState["Requester_List"].AttemptedValue;
                newObj.Organization_Name = ModelState["Organization_Name"].AttemptedValue;
                newObj.Email = ModelState["Email"].AttemptedValue;
                newObj.Role = ModelState["Role"].AttemptedValue;
                newObj.Subject = ModelState["Subject"].AttemptedValue;
                newObj.Description = ModelState["Description"].AttemptedValue;
                newObj.InternalNote = string.IsNullOrEmpty(ModelState["InternalNote"].AttemptedValue) ? "" : ModelState["InternalNote"].AttemptedValue;
                newObj.Category = Convert.ToInt32(ModelState["Category_List"].AttemptedValue);
                newObj.RequestType = Convert.ToInt32(ModelState["Request_List"].AttemptedValue);
                newObj.Priority = Convert.ToInt32(ModelState["RequestPriority_List"].AttemptedValue);
                newObj.Assigned_QMS = ModelState["AssigneeQMS_List"].AttemptedValue;
                newObj.Assigned_Programmer = ModelState["AssigneeProg_List"].AttemptedValue;
                newObj.Status = 1;
                newObj.CreatedBy = HttpContext.Session.GetString("AccessCode");
            }

            try
            {
                DBHelper dbHelper = new();

                dbHelper.CreateNewTicket(newObj); //Call Function, insert into database
                dbHelper.CreateNewTicketSub(newObj); //Call Function, insert into database

                sequenceServices.SequenceCodeUpdate("Ticket_ID");

                ViewBag.Alert = AlertServices.ShowAlert(Alerts.Success, "New Ticket Added!");
                ModelState.Clear();

                return PartialView("NewTicket", GetListofValue());
            }
            catch (Exception ex)
            {
                return RedirectToAction("NewTicket", ex.Message);
            }
        }
    }
}
