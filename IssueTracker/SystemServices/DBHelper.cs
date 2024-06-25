using System;
using System.Data;
using Microsoft.Data.SqlClient;

using IssueTracker.Models;
using IssueTracker.SystemModels;

namespace IssueTracker.SystemServices
{
    public class DBHelper
    {
        public static IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            return builder;
        }

        //Create New Ticket
        public string CreateNewTicket(NewTicketModel newObj)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@TicketID", newObj.TicketID),
                new SqlParameter("@Subject", newObj.Subject),
                new SqlParameter("@Description", newObj.Description),
                new SqlParameter("@InternalNote", newObj.InternalNote),
                new SqlParameter("@Category", newObj.Category),
                new SqlParameter("@RequestType", newObj.RequestType),
                new SqlParameter("@Priority", newObj.Priority),
                new SqlParameter("@Assigned_QMS", newObj.Assigned_QMS),
                new SqlParameter("@Assigned_Programmer", newObj.Assigned_Programmer),
                new SqlParameter("@Requester", newObj.Requester),
                new SqlParameter("@Status", newObj.Status),
                new SqlParameter("@CreatedBy", newObj.CreatedBy)
            };

            try
            {
                return DBHelper.ExecuteNonQuery("sp_NewTicket_Insert", CommandType.StoredProcedure, parameter);
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Create New Ticket Sub
        public string CreateNewTicketSub(NewTicketModel newObj)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@TicketID", newObj.TicketID),
                new SqlParameter("@RequesterID", newObj.Requester),
                new SqlParameter("@Organization", newObj.Organization_Name),
                new SqlParameter("@Email", newObj.Email),
                new SqlParameter("@Role", newObj.Role),
                new SqlParameter("@CreatedBy", newObj.CreatedBy)
            };

            try
            {
                return DBHelper.ExecuteNonQuery("sp_NewTicketSub_Insert", CommandType.StoredProcedure, parameter);
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static DataSet GetData(string strCommand, CommandType cmdType)
        {
            var getConn = GetConnection().GetSection("ConnectionStrings").GetSection("MSSQLSERVER2023").Value;
            SqlConnection cnn = new(getConn);

            DataSet ResultSet = new();
            SqlDataAdapter SQLAdap = new();
            try
            {
                cnn.Open();
                SQLAdap.SelectCommand = new SqlCommand(strCommand, cnn);
                SQLAdap.Fill(ResultSet);

                return ResultSet;
            }

            finally
            {
                cnn.Close();
            }
        }

        static internal string ExecuteNonQuery(string CommandName, CommandType cmdType, SqlParameter[] param)
        {
            var getConn = GetConnection().GetSection("ConnectionStrings").GetSection("MSSQLSERVER2023").Value;
            SqlConnection cnn = new(getConn);
            _ = new DataSet();

            try
            {
                cnn.Open();

                SqlCommand cmd = new(CommandName, cnn)
                {
                    CommandType = cmdType,
                    CommandText = CommandName
                };
                cmd.Parameters.AddRange(param);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    return AlertServices.ShowAlert(Alerts.Success, "New Ticket Added!");
                }
                else
                {
                    return "an error occured transaction not posted.";
                }
            }

            finally
            {
                cnn.Close();
            } 
        }

        public static DataSet ExecuteParamerizedReader(string CommandName, CommandType CmdType, SqlParameter[] param)
        {
            var getConn = GetConnection().GetSection("ConnectionStrings").GetSection("MSSQLSERVER2023").Value;
            SqlConnection cnn = new SqlConnection(getConn);

            DataSet ds = new();

            try
            {
                cnn.Open();
                SqlCommand cmd = new(CommandName, cnn)
                {
                    CommandType = CmdType,
                    CommandText = CommandName
                };
                cmd.Parameters.AddRange(param);

                SqlDataAdapter SQLlAdap = new(cmd);
                SQLlAdap.Fill(ds);

                return ds;
            }

            finally
            {
                cnn.Close();
            }

        }
    }
}
