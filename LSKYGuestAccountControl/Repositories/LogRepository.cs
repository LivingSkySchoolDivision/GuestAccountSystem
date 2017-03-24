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
                UserAgent = dataReader["UserAgent"].ToString(),
                BatchID = dataReader["batchid"].ToString(),
                Password = dataReader["password"].ToString()
            };
        }
        public List<LoggedActivation> GetBatchID(string batchID)
        {
            List<LoggedActivation> returnMe = new List<LoggedActivation>();
            if (!string.IsNullOrEmpty(batchID))
            {
                using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_Internal))
                {
                    SqlCommand sqlCommand = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = "SELECT * FROM log_activation WHERE batchid=@BATCHID ORDER BY LogDate DESC;"
                    };
                    sqlCommand.Parameters.AddWithValue("@BATCHID", batchID);
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
            }
            return returnMe;
        }
        public List<LoggedActivation> GetActivationsToday(LoginSession currentUser)
        {
            // Date range is always today
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(1).AddMinutes(-1);

            List<LoggedActivation> returnMe = new List<LoggedActivation>();
            if (currentUser != null)
            {
                using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_Internal))
                {
                    SqlCommand sqlCommand = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = "SELECT * FROM log_activation WHERE RequestingUser=@RUSER AND LogDate>=@STARDATE AND LogDate<=@ENDDATE ORDER BY LogDate DESC;"
                    };
                    sqlCommand.Parameters.AddWithValue("@RUSER", currentUser.Username);
                    sqlCommand.Parameters.AddWithValue("@STARDATE", startDate);
                    sqlCommand.Parameters.AddWithValue("@ENDDATE", endDate);
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
            }
            return returnMe;
        }

        public List<LoggedActivation> GetRecentEntries(int max)
        {
            List<LoggedActivation> returnMe = new List<LoggedActivation>();
            if (max > 0)
            {
                using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_Internal))
                {
                    SqlCommand sqlCommand = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = "SELECT TOP " + max + " * FROM log_activation ORDER BY LogDate DESC;"
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
            }
            return returnMe;
        }
        
        public void LogActivation(string batchID, GuestAccount guestAccount, LoginSession currentUser, string reason)
        {
            using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_Internal))
            {
                using (SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.Text,
                    CommandText = "INSERT INTO log_activation(LogDate, GuestAccountName, RequestingUser, Reason, IPAddress, UserAgent, Password, batchid) " +
                                  "                    VALUES(@LOGDATE, @GUESTNAME, @REQUSER, @REASON, @IPADDR, @USERAGENT, @PASSWD, @BATCHID);"
                })
                {
                    sqlCommand.Parameters.AddWithValue("@LOGDATE", DateTime.Now.ToString());
                    sqlCommand.Parameters.AddWithValue("@GUESTNAME", guestAccount.sAMAccountName);
                    sqlCommand.Parameters.AddWithValue("@REQUSER", currentUser.Username);
                    sqlCommand.Parameters.AddWithValue("@REASON", reason);
                    sqlCommand.Parameters.AddWithValue("@IPADDR", currentUser.IPAddress);
                    sqlCommand.Parameters.AddWithValue("@USERAGENT", currentUser.UserAgent);
                    sqlCommand.Parameters.AddWithValue("@PASSWD", guestAccount.Password);
                    sqlCommand.Parameters.AddWithValue("@BATCHID", batchID);

                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
            
        }

    }
}