using System;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;

namespace habit_tracker // Living in a namespace to avoid conflicts
{
    
    class Program
    {
        static string connectionString = @"Data Source=habit-tracker.db"; // String to connect to the right database
        static void Main(string[] args) // Main
        {

            // 'using' will close this at the end, making sure it's picked up by the garbage collector
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open(); // Open table
                var tableCommand = connection.CreateCommand(); // Create a command

                // Set the text of the command (SQL)
                tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ""Date"" TEXT,
                Quantity INTEGER)";
                tableCommand.ExecuteNonQuery(); // Execute (non-query) command
                connection.Close(); // Close connection manually
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
            string? date = GetDateInput(); // string? implies a variable that can either be null or an actual string
            if(date == null){ return; }

            int? quantity = GetNumberInput(); // same for int?
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

        internal static string? GetDateInput() // ? can also be used as a return type
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

            int finalInput = Convert.ToInt32(numberInput); // Converting our input into an integer
            return finalInput;
        }

        // Function to view all records
        private static void GetAllRecords()
        {
            using (var connection = new SqliteConnection(connectionString)) // New connection
            {
                // Open, create command, set text
                connection.Open();
                var tableCommand = connection.CreateCommand();
                tableCommand.CommandText = $"SELECT * FROM drinking_water";

                // Make a list of our data
                List<DrinkingWater> tableData = new List<DrinkingWater>();
                // Make the reader read every row of the table
                SqliteDataReader reader = tableCommand.ExecuteReader();

                if (reader.HasRows) // If our reader has any rows
                {
                    while (reader.Read()) // While we read each row
                    {
                        // Add the data to our DrinkingWater list
                        tableData.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0), // 0 represents the column of our entry
                                Date = DateTime.Parse(reader.GetString(1)), // Parsing a string as proper datetime
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
                foreach (var entry in tableData) // For every entry in the table, print it out to the console
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
        
        private static void Update()
        {
            Console.Clear();

            GetAllRecords();

            Console.WriteLine("\nType the ID of the record you wish to change\n");
            int input = Convert.ToInt32(Console.ReadLine());


            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE id = {input})";
                int checkQuery = Convert.ToInt32(checkCommand.ExecuteScalar());

                if(checkQuery == 0)
                {
                    Console.WriteLine("\nNo records to update!\n");
                    connection.Close();
                }


                string? date = GetDateInput();
                int? quantity = GetNumberInput();

                var tableCommand = connection.CreateCommand();
                tableCommand.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {input}";

                tableCommand.ExecuteNonQuery();
                connection.Close();
            }

        }
    }

}

// Basic class to hold entries of DrinkingWater
public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}