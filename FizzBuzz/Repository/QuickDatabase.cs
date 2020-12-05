using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FizzBuzz.Models;

namespace FizzBuzz.Repository
{

    /* QuickDatabase Schema
        CREATE TABLE dbo.FizzBuzzLog(
	    FizzBuzzLogID INT IDENTITY(1,1) PRIMARY KEY,
	    LogDate DATETIME NOT NULL,
	    Results NVARCHAR(MAX) NOT NULL,
	    StatusCode INT NOT NULL,
	    INDEX IX_FizzBuzzLog_LogDate_StatusCode NONCLUSTERED (LogDate, StatusCode));
     */

    //Would be quicker and easier to implement Entity Framework, but let's do some OG SQL. ;) 
    public static class QuickDatabase
    {
        private const string connectionString = @"Data Source=<INSERT MACHINE NAME HERE>;Initial Catalog=Fizzbuzz;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
        
        private const string insertLogStr = @"INSERT INTO dbo.FizzBuzzLog (LogDate, Results, StatusCode) VALUES (GETDATE(), @Results, @StatusCode);";
        private const string getLogsStr = @"SELECT FizzBuzzLogID, LogDate, Results, StatusCode FROM dbo.FizzBuzzLog;";
        private const string getLogsByIdStr = @"SELECT FizzBuzzLogID, LogDate, Results, StatusCode FROM dbo.FizzBuzzLog WHERE FizzBuzzLogID = @ID;";

        public static string WriteFbLog(FizzBuzzLogDto fbl)
        {
            string responseMessage = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(insertLogStr, connection);
                                        
                    cmd.Parameters.AddWithValue("@Results", fbl.Results);
                    cmd.Parameters.AddWithValue("@StatusCode", fbl.StatusCode);

                    cmd.Connection.Open();

                    var result = cmd.ExecuteNonQuery();

                    cmd.Connection.Close();
                    responseMessage = "success";
                }
                catch (Exception e)
                {
                    responseMessage = e.Message;
                }
            }
            return responseMessage;
        }

        public static List<FizzBuzzLogDto> GetLogs()
        {
            List<FizzBuzzLogDto> results = new List<FizzBuzzLogDto>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {                
                try
                {
                    SqlCommand cmd = new SqlCommand(getLogsStr, connection);
                    cmd.Connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var fbLog = MapResultToDto(reader);
                        results.Add(fbLog);
                    }

                    cmd.Connection.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            return results;
        }

        public static FizzBuzzLogDto GetLogById(int Id)
        {
            FizzBuzzLogDto fbLog = new FizzBuzzLogDto();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(getLogsByIdStr, connection);
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.Connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        fbLog = MapResultToDto(reader);
                    }

                    cmd.Connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            return fbLog;
        }

        private static FizzBuzzLogDto MapResultToDto(SqlDataReader reader)
        {
            var log = new FizzBuzzLogDto
            {
                ID = (int)reader["FizzBuzzLogID"],
                LogDate = (DateTime)reader["LogDate"],
                Results = (string)reader["Results"],
                StatusCode = (int)reader["StatusCode"]
            };

            return log;
        }
    }
}
