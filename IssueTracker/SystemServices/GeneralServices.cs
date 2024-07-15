using System.Data;
using Microsoft.Data.SqlClient;

namespace IssueTracker.SystemServices
{
    public class GeneralServices
    {
        //Get User Role Name [Global]
        public DataSet GetRoleName(int roleID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@roleID", roleID)
            };

            return DBHelper.ExecuteParamerizedReader("sp_GetUserRoleName", CommandType.StoredProcedure, myparams);
        }

        //Get Priority Count - Chart [Dashboard]
        public DataSet GetPriorityCounts(string strParamName)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strParameterName", strParamName)
            };

            return DBHelper.ExecuteParamerizedReader("sp_GetPriorityCountChart", CommandType.StoredProcedure, myparams);
        }

        //Get Open Tickets List [Dashboard]
        public DataSet GetOpenTickets(string strCreatedBy, int roleId)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strCreatedBy", strCreatedBy),
                new SqlParameter("@strParamName", roleId)
            };

            return DBHelper.ExecuteParamerizedReader("sp_GetOpenTickets", CommandType.StoredProcedure, myparams);
        }

        //Get Tickets List [Tickets]
        public DataSet GetTicketList()
        {

            return DBHelper.GetData("sp_GetTicketList", CommandType.StoredProcedure);
        }

        //Get Ticket Details [Ticket Details]
        public DataSet GetTicketDetails(string strTicketID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strTicketID", strTicketID)
            };

            return DBHelper.ExecuteParamerizedReader("sp_GetTicketDetails", CommandType.StoredProcedure, myparams);
        }

        //Get Account Settings Information [Settings]
        public DataSet GetAccountSettings(string accessCode, string strStatement)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@accessCode", accessCode),
                new SqlParameter("@strStatement", strStatement)
            };

            return DBHelper.ExecuteParamerizedReader("sp_GetAccountSettings", CommandType.StoredProcedure, myparams);
        }
    }
}
