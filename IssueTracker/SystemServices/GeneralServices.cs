using System.Data;
using Microsoft.Data.SqlClient;

namespace IssueTracker.SystemServices
{
    public class GeneralServices
    {
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
        public DataSet GetOpenTickets(string strCreatedBy)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strCreatedBy", strCreatedBy)
            };

            return DBHelper.ExecuteParamerizedReader("sp_GetOpenTickets", CommandType.StoredProcedure, myparams);
        }

        //Get Tickets List where created by [Tickets]
        public DataSet GetTicketsbyCreatedBy(string strCreatedBy)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strCreatedBy", strCreatedBy)
            };

            return DBHelper.ExecuteParamerizedReader("sp_GetTicketList", CommandType.StoredProcedure, myparams);
        }

        //Get Ticket Details [Ticket Details]
        public DataSet GetTicketDetails(string strTicketID, string strCreatedBy)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strTicketID", strTicketID),
                new SqlParameter("@strCreatedBy", strCreatedBy)
            };

            return DBHelper.ExecuteParamerizedReader("sp_GetTicketDetails", CommandType.StoredProcedure, myparams);
        }
    }
}
