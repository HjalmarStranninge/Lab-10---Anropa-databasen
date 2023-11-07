using Lab_10___Anropa_databasen.Data;
using Lab_10___Anropa_databasen.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net;
using System.Numerics;

namespace Lab_10___Anropa_databasen
{
    internal class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                // Creating a simple menu.

                Console.Clear();
                Console.WriteLine("[1]: List all customers");
                Console.WriteLine("[2]: Add new customer");
                Console.WriteLine("[3]: Exit");

                Console.Write("Enter choice: ");
                string menuChoice = Console.ReadLine();

                switch (menuChoice)
                {                   
                    case "1":

                        // Asks the user in what order the list should be listed, 1 for asc and 2 for desc.
                        // The user is continously prompted until either option is picked.

                        int userChoice = 0;
                        while (userChoice != 1 && userChoice != 2)
                        {
                            Console.Clear();
                            Console.WriteLine("Order list by [1]: Ascending or [2]: Descending?");

                            // Checks for invalid input to prevent crashing.

                            try
                            {
                                userChoice = int.Parse(Console.ReadLine());
                            }
                            catch
                            {
                                Console.Clear();
                                Console.Write("Invalid input!");
                                Thread.Sleep(1000);
                            }
                        }

                        // Displays all the customers in a list.

                        DisplayCustomers(userChoice);

                        // The user is then asked to pick a customer in the list to display more info about that customer.

                        Console.Write("Select customer number to view more info: ");
                        try
                        {
                            userChoice = int.Parse(Console.ReadLine());
                        }
                        
                        // If you enter something that would normally crash the program,
                        // the 'userChoice'-variable is set to an impossibly large number so that the search executes but never returns a result.

                        catch
                        {
                            userChoice = 1000000000;
                        }

                        DisplayAllCustomerInfo(userChoice);

                    break;       
                        
                    case "2":
                        
                        AddCustomer();
                    break;

                    case "3":

                        Environment.Exit(0);
                    break;

                    default:

                        Console.Clear();
                        Console.Write("Invalid input!");
                        Thread.Sleep(1000);
                    break;
                }
            }
        }


        // Method for listing all the customers.
        static void DisplayCustomers(int order)
        {
            Console.Clear();

            using (var context = new NorthWindContext())
            {
                List<Customer> customers;

                // Depending on user input, the list is ordered in either ascending or descending order.

                if (order == 1)
                {
                    customers = context.Customers
                                .Include(c => c.Orders)
                                .OrderBy(c => c.CompanyName)
                                .ToList();
                }

                else
                {
                    customers = context.Customers
                                .Include(c => c.Orders)
                                .OrderByDescending(c => c.CompanyName)
                                .ToList();
                }

                // Using a for-loop to iterate through the list.
                // This gives each customer a unique number which can be used to easily search for a specific customer later.

                for (var i = 0; i < customers.Count(); i++)
                {
                    Console.WriteLine($"{i}. {customers.ElementAt(i).CompanyName}\n\n" +
                        $"COUNTRY/REGION: {customers.ElementAt(i).Country}/{customers.ElementAt(i).Region}\n" +
                        $"PHONE NUMBER: {customers.ElementAt(i).Phone}\n" +
                        $"AMOUNT OF ORDERS: {customers.ElementAt(i).Orders.Count()}");
                    Console.WriteLine("--------------------\n");
                }
            }
        }


        // Method for displaying all the customer info for a specific customer.
        static void DisplayAllCustomerInfo(int search)
        {
            // Resetting the cursor position before clearing, the second line is used to clear the scrollback buffer.

            Console.Clear(); 
            Console.Write("\x1b[3J");
            Console.Write("Fetching customer information, please wait...");
                
            using (var context = new NorthWindContext())
            {
                var customers = context.Customers
                                .Include(c => c.Orders)
                                .ThenInclude(c=> c.OrderDetails)
                                .ThenInclude(c => c.Product)
                                .ToList();

                // Variable for checking if a customer was found, and throwing an error message if not.

                bool customerFound = false;

                // Since we iterate through the list in the same manner as before, each customer will have the same unique index number.
                // This index number becomes kind of a unique customer ID visible to the user,
                // so that they can easily search for a specific customer in the list.

                for (var i = 0; i < customers.Count(); i++)
                {
                    if (search == i)
                    {

                        // All info that is available about the customer is displayed.
                        Console.Clear();
                        Console.WriteLine($"{i}. {customers.ElementAt(i).CompanyName}\n\n" +
                            $"ADDRESS: {customers.ElementAt(i).Address} -  {customers.ElementAt(i).PostalCode} {customers.ElementAt(i).City}\n" +
                            $"COUNTRY/REGION: {customers.ElementAt(i).Country} / {customers.ElementAt(i).Region}\n" +
                            $"CONTACT PERSON: {customers.ElementAt(i).ContactName} - {customers.ElementAt(i).ContactTitle}\n" +
                            $"PHONE/FAX: {customers.ElementAt(i).Phone} / {customers.ElementAt(i).Fax}\n\n" +
                            $"\tALL ORDERS: \n");

                        // Iterating through all orders, showing order id and date.
                        foreach(var order in customers[i].Orders)
                        {
                            Console.WriteLine($"Order: {order.OrderId}");
                            Console.WriteLine($"Order date: {order.OrderDate}");
                            Console.WriteLine();

                            // For each order, we iterate 
                            foreach (var orderDetail in order.OrderDetails)
                            {
                                Console.WriteLine($"Product name: {orderDetail.Product.ProductName}");
                                Console.WriteLine($"Price per unit: {(int)orderDetail.UnitPrice}:-");
                                Console.WriteLine($"Amount on order: {orderDetail.Quantity}");
                                Console.WriteLine();
                            }
                            Console.WriteLine("\n");
                        }

                        Console.Write("Press ENTER to continue: ");
                        Console.ReadLine();

                        customerFound = true;
                    }
                }

                if (!customerFound)
                {
                    Console.Clear();
                    Console.WriteLine("The customer number you searched for wasn't found.");
                    Console.Write("Press ENTER to continue: ");
                    Console.ReadLine();
                }
            }
        }


        // Method for adding new customers to the database.
        static void AddCustomer()
        {
            Console.Clear();
            Console.WriteLine("Creating a new customer profile. You will be asked for customer details. " +
                   "Press ENTER to leave a field empty.");
            Console.WriteLine();

            using (var context = new NorthWindContext())
            {

                // User is asked to enter info. A custom method is used to set the property and set it to 'null' if nothing is entered.

                Customer customer = new Customer();

                Console.Write("Enter company name: ");
                customer.CompanyName = SetProperty();

                Console.Write("Enter contact name: ");
                customer.ContactName = SetProperty();

                Console.Write("Enter contact title: ");
                customer.ContactTitle = SetProperty();

                Console.Write("Enter address: ");
                customer.Address = SetProperty();

                Console.Write("Enter city: ");
                customer.City = SetProperty();

                Console.Write("Enter region: ");
                customer.Region = SetProperty();

                Console.Write("Enter postal code: ");
                customer.PostalCode = SetProperty();

                Console.Write("Enter country: ");
                customer.Country = SetProperty();

                Console.Write("Enter phone number: ");
                customer.Phone = SetProperty();

                Console.Write("Enter fax: ");
                customer.Fax = SetProperty();

                customer.CustomerId = GenerateID(customer.CompanyName);

                context.Customers.Add(customer);
                context.SaveChanges();

                Console.WriteLine("Customer added!");
            }


        // Method for checking if the user entered a value and returning that value, if nothing is entered, it returns null.

        static string SetProperty()
        {
            string userInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(userInput))
            {
                return userInput;
            }
            else
            {
                return null;
            }
        }


        // Generates an ID based on the name by removing all spaces and capitalizing the whole name extracting the 5 first letters.

        static string GenerateID(string name)
        {
            name = name.Replace(" ", "").ToUpper();

            var characters = name.ToList();

            string id = new string(characters.Take(5).ToArray());

            return id;
        }
        }
    }
}