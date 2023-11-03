using Lab_10___Anropa_databasen.Data;
using Lab_10___Anropa_databasen.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Lab_10___Anropa_databasen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Creating a simple menu.

            Console.WriteLine("[1]: List all customers");
            Console.WriteLine("[2]: Add new customer");

            Console.Write("Enter choice: ");
            string menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":

                    // Creating an instance of NorthwindContext.

                    using (var context = new NorthWindContext())
                    {
                        // Asks the user in what order the list should be listed.
                        Console.Clear();
                        Console.WriteLine("List [1]: Ascending or [2]: Descending?");

                        int userChoice = int.Parse(Console.ReadLine());

                        if (userChoice == 1)
                        {
                            // Creating an anonymous object, ordered by company name ascending.

                            Console.Clear();
                            var customers = context.Customers
                            .Include(c => c.Orders)
                            .OrderBy(c => c.CompanyName)
                            .ToList();                           

                            DisplayCustomers(customers);
                       
                            Console.Write("Select customer number to view more info: ");
                            userChoice = int.Parse(Console.ReadLine());

                            DisplayAllCustomerInfo(customers, userChoice);                       
                        }
                        
                        // Same code as above, but with the list being ordered descending instead.
                        if (userChoice == 2)
                        {
                            Console.Clear();
                            var customers = context.Customers
                            .Include(c => c.Orders)
                            .OrderByDescending(c => c.CompanyName)
                            .ToList();

                            DisplayCustomers(customers);

                            Console.Write("Select customer number to view more info: ");
                            userChoice = int.Parse(Console.ReadLine());

                            DisplayAllCustomerInfo(customers, userChoice);
                        }                       
                    }

                break;

            }
        } 

        // Method for listing all the customers.
        static void DisplayCustomers(IEnumerable<Customer> customers)
        {
            for (var i = 0; i < customers.Count(); i++)
            {
                Console.WriteLine($"{i}. {customers.ElementAt(i).CompanyName}\n\n" +
                    $"COUNTRY/REGION: {customers.ElementAt(i).Country}/{customers.ElementAt(i).Region}\n" +
                    $"PHONE NUMBER: {customers.ElementAt(i).Phone}\n" +
                    $"AMOUNT OF ORDERS: {customers.ElementAt(i).Orders.Count()}");
                Console.WriteLine("--------------------\n");
            }
        }
        // Method for displaying all the customer info for a specific customer.
        static void DisplayAllCustomerInfo(IEnumerable<Customer> customers, int search)
        {
            bool customerFound = false;
            for (var i = 0; i < customers.Count(); i++)
            {
                if (search == i)
                {
                    Console.Clear();
                    Console.WriteLine($"{i}. {customers.ElementAt(i).CompanyName}\n\n" +
                        $"ADDRESS: {customers.ElementAt(i).Address} -  {customers.ElementAt(i).PostalCode} {customers.ElementAt(i).City}\n" +
                        $"COUNTRY/REGION: {customers.ElementAt(i).Country} / {customers.ElementAt(i).Region}\n" +
                        $"CONTACT PERSON: {customers.ElementAt(i).ContactName} - {customers.ElementAt(i).ContactTitle}\n" +
                        $"PHONE/FAX: {customers.ElementAt(i).Phone} / {customers.ElementAt(i).Fax}\n" +
                        $"AMOUNT OF ORDERS: {customers.ElementAt(i).Orders.Count()}");

                    customerFound = true;
                }
            }

            if (!customerFound)
            {
                Console.Clear();
                Console.WriteLine("The customer number you searched for wasn't found.");
            }
        }
       

                
        
    }
}