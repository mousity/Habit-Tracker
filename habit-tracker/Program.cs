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

                tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ""Date"" TEXT,
                Quantity INTEGER)";
                tableCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while(closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\n What would you like to do?");
                Console.WriteLine("\nType 0 to close the application");
                Console.WriteLine("Type 1 to view all records");
                Console.WriteLine("Type 2 to insert record");
                Console.WriteLine("Type 3 to delete record");
                Console.WriteLine("Type 4 to update record");
                Console.WriteLine("--------------------------");
            }
        }
    }

}

