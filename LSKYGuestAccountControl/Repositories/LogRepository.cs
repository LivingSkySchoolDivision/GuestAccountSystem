using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LSKYGuestAccountControl.Model;
using LSKYGuestAccountControl.Static;

namespace LSKYGuestAccountControl.Repositories
{
    public class LogRepository
    {
        private LoggedActivation dataReaderToLoggedActivation(SqlDataReader dataReader)
        {
            return new LoggedActivation()
            {
                Date = Parsers.ParseDate(dataReader["LogDate"].ToString()),
                GuestAccountName = dataReader["GuestAccountName"].ToString(),
                RequestingUser = dataReader["RequestingUser"].ToString(),
                Reason = dataReader["Reason"].ToString(),
                IPAddress = dataReader["IPAddress"].ToString(),
                UserAgent = dataReader["UserAgent"].ToString()
            };
        }


        public List<LoggedActivation> GetRecentEntries(int max)
        {
            List<LoggedActivation> returnMe = new List<LoggedActivation>();

            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_Internal))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM log_activation;"
                };
                sqlCommand.Connection.Open();
                SqlDataReader dataReader = sqlCommand.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        returnMe.Add(dataReaderToLoggedActivation(dataReader));
                    }
                }
                sqlCommand.Connection.Close();
            }

            return returnMe;
        }

        public void LogActivation(GuestAccount guestAccount, LoginSession currentUser, string reason)
        {
            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_Internal))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "INSERT INTO log_activation(LogDate, GuestAccountName, RequestingUser, Reason, IPAddress, UserAgent) " +
                                  "                    VALUES(@LOGDATE, @GUESTNAME, @REQUSER, @REASON, @IPADDR, @USERAGENT);"
                })
                {
                    sqlCommand.Parameters.AddWithValue("@LOGDATE", DateTime.Now.ToString());
                    sqlCommand.Parameters.AddWithValue("@GUESTNAME", guestAccount.sAMAccountName);
                    sqlCommand.Parameters.AddWithValue("@REQUSER", currentUser.Username);
                    sqlCommand.Parameters.AddWithValue("@REASON", reason);
                    sqlCommand.Parameters.AddWithValue("@IPADDR", currentUser.IPAddress);
                    sqlCommand.Parameters.AddWithValue("@USERAGENT", currentUser.UserAgent);

                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            
        }

    }
}