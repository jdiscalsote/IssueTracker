using System;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;

using IssueTracker.SystemModels;

namespace IssueTracker.SystemServices
{
    public class SequenceServices
    {
        //For Generate
        public SequenceModel SequenceCodeGenerate(string strSequenceName)
        {
            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@strSequenceName", strSequenceName)
            };

            DataSet dsSEQ = DBHelper.ExecuteParamerizedReader("sp_GetSequenceInfo", CommandType.StoredProcedure, parameter);
            SequenceModel sequence = new();

            if ((dsSEQ.Tables[0].Rows.Count > 0))
            {
                //Sequence no.
                string strFormat = ((dsSEQ.Tables[0].Rows[0]["format"] == System.DBNull.Value) ? "" : dsSEQ.Tables[0].Rows[0]["format"]).ToString();
                int intLastSeqCode = Convert.ToInt32(((dsSEQ.Tables[0].Rows[0]["next"] == System.DBNull.Value) ? "" : dsSEQ.Tables[0].Rows[0]["next"]).ToString());

                //Generate new sequence
                int intCodeLength = 6;

                int intNewSeq = Convert.ToInt32(intLastSeqCode) + 1;

                //Create StringBuilder for sequence construction
                StringBuilder strBuilder = new StringBuilder();

                //Determine the number of zeroes to include based on the length of intNewSeq
                int intSeqCounter = intCodeLength - Convert.ToString(intNewSeq).Length;

                for (int i = 0; i < intSeqCounter; i++)
                {
                    strBuilder.Append('0');
                }

                // Append the new sequence number
                strBuilder.Append(intNewSeq);

                // Assign the sequence to sequence.NewSequence using strFormat
                sequence.NewSequence = strFormat + strBuilder.ToString();
            }
            else
            {
                sequence.SequenceType = "";
            }


            return sequence;
        }

        //For Update
        public string SequenceCodeUpdate(string strSequenceName)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
               new SqlParameter("@strSequenceName", strSequenceName)
            };

            return DBHelper.ExecuteNonQuery("sp_UpdateSequenceInfo", CommandType.StoredProcedure, myparams);
        }
    }
}
