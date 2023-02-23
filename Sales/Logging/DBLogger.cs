using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Sales.Logging
{
    internal class DBLogger : ILogger
    {
        private readonly SqlConnection _connection;
        public DBLogger(String connectionString) 
        {
            _connection = new(connectionString);
            _connection.Open();
            
        }
        public void Log(String message)
        {
            this.Log(message, LogLevel.Debug);
        }

        public void Log(String message, LogLevel Level)
        {
            this.Log(message, Level, "", "");
        }

        public void Log(String message, LogLevel Level, String ClassName, String MetodName)
        {
            this.Log(message, Level, ClassName, MetodName, null);
        }

        public void Log(String message, LogLevel Level, String ClassName, String MethodName, object info)
        {
            String sql = "INSERT INTO Logs([logDT],[message],[level],[className],[methodName],[info]) " +
  "VALUES(CURRENT_TIMESTAMP, @message, @level, @className, @methodName, @info)";
            using SqlCommand cmd = new(sql, _connection);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@level", Level.ToString().ToUpper()[0]);
            cmd.Parameters.AddWithValue("@className", ClassName == "" ? DBNull.Value : ClassName);
            cmd.Parameters.AddWithValue("@methodName", MethodName == "" ? DBNull.Value : MethodName);
            cmd.Parameters.AddWithValue("@info", info == null ? DBNull.Value : info);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
         
        }
    }
}
