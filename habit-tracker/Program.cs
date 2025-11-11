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
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\n What would you like to do?");
                Console.WriteLine("\nType 0 to close the application");
                Console.WriteLine("Type 1 to view all records");
                Console.WriteLine("Type 2 to insert record");
                Console.WriteLine("Type 3 to delete record");
                Console.WriteLine("Type 4 to update record");
                Console.WriteLine("--------------------------");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case 0:
                        Console.WriteLine("Goodbye!");
                        closeApp = true;
                        break;
                    case 1:
                        GetAllRecords();
                        break;
                    case 2:
                        Insert();
                        break;
                    case 3:
                        Delete();
                        break;
                    case 4:
                        Update();
                        break;
                    default:
                        Console.WriteLine("Invalid command. Please type in a number from 0 to 4.");
                        break;
                }
            }
        }

        private static void Insert()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();
                tableCommand.CommandText =
                $"INSERT INTO drinking_water(date, quantity) VALUES('{date}',{quantity})";

                tableCommand.ExecuteNonQuery();

                connection.Close();

            }


        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: dd-mm-yy format. Type 0 to return to the main menu");

            string dateInput = Console.ReadLine();
            if (dateInput == "0") { GetUserInput(); }

            return dateInput;
        }
        
        internal static int GetNumberInput()
        {
            Console.WriteLine("\n\nPlease type the number of glasses you had today, or any other unit you desire\n");

            string numberInput = Console.ReadLine();
            if (numberInput == "0") { GetUserInput(); }

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }

}

