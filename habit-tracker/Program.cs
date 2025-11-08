using System;
using Microsoft.Data.Sqlite;

namespace habit_tracker
{
    
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=habit-tracker.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = "";
                tableCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

}

