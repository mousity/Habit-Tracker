using System;
using Microsoft.Data.Sqlite;

namespace habit_tracker
{
    
    class Program
    {
        static string connectionString = @"Data Source=habit-tracker.db";
        static void Main(string[] args)
        {

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

            GetUserInput();
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

                int commandInput = Convert.ToInt32(Console.ReadLine());

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
                    // case 4:
                    //     Update();
                    //     break;
                    default:
                        Console.WriteLine("Invalid command. Please type in a number from 0 to 4.");
                        break;
                }
            }
        }

        private static void Insert()
        {
            string? date = GetDateInput();
            if(date == null){ return; }

            int? quantity = GetNumberInput();
            if(quantity == null){ return; }

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

        internal static string? GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: dd-mm-yy format. Type 0 to return to the main menu");

            string dateInput = Console.ReadLine();
            if (dateInput == "0") { return null; }

            return dateInput;
        }

        internal static int? GetNumberInput()
        {
            Console.WriteLine("\n\nPlease type the number of glasses you had today, or any other unit you desire\n");

            string numberInput = Console.ReadLine();
            if (numberInput == "0") { return null; }

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }

        private static void GetAllRecords()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();
                tableCommand.CommandText = $"SELECT * FROM drinking_water";
                List<DrinkingWater> tableData = new List<DrinkingWater>();
                SqliteDataReader reader = tableCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.Parse(reader.GetString(1)),
                                Quantity = reader.GetInt32(2)
                            }
                        ); ;
                    }
                }
                else
                {
                    Console.WriteLine("No entries!");
                }

                connection.Close();

                Console.WriteLine("----------------------------");
                foreach (var entry in tableData)
                {
                    Console.WriteLine($"{entry.Id} - {entry.Date.ToString("dd-MM-yyyy")} - Quantity: {entry.Quantity}");
                }
                Console.WriteLine("----------------------------");
            }
        }
        
        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            Console.WriteLine("\nWhich record would you like to delete? Please type in the record ID. If you want to return to the main menu, press 0.");

            int input = Convert.ToInt32(Console.ReadLine());

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();
                tableCommand.CommandText = $"DELETE from drinking_water WHERE id = '{input}'";
                int rowCount = tableCommand.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine("\nNo records to delete!\n");
                }

                connection.Close();

            }
        }
    }

}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}