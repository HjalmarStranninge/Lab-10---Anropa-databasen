using Lab_10___Anropa_databasen.Data;
using Lab_10___Anropa_databasen.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Lab_10___Anropa_databasen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new NorthWindContext())
            {
                var customers = context.Customers
                    .Include(c=> c.Orders)
                    .OrderBy(c => c.CompanyName)
                    .ToList();

                for (var i = 1; i < customers.Count; i++) 
                {
                    Console.WriteLine($"{i}. {customers[i].CompanyName}\n\n" +
                        $"COUNTRY/REGION: {customers[i].Country}/{customers[i].Region}\n" +
                        $"PHONE NUMBER: {customers[i].Phone}\n" +
                        $"AMOUNT OF ORDERS: {customers[i].Orders.Count()}");
                    Console.WriteLine("--------------------\n");
                }

                Console.Write("Select customer number to view more info: ");
                int userChoice = int.Parse(Console.ReadLine());

                for (var i = 1; i < customers.Count; i++)
                {
                    if (userChoice == i)
                    {
                        Console.Clear();
                        Console.WriteLine($"{i}. {customers[i].CompanyName}\n\n" + 
                            $"ADDRESS: {customers[i].Address} -  {customers[i].PostalCode} {customers[i].City}\n" +
                            $"COUNTRY/REGION: {customers[i].Country} / {customers[i].Region}\n" +
                            $"CONTACT PERSON: {customers[i].ContactName} - {customers[i].ContactTitle}\n" +
                            $"PHONE/FAX: {customers[i].Phone} / {customers[i].Fax}\n" +
                            $"AMOUNT OF ORDERS: {customers[i].Orders.Count()}");
                    }

                    else
                    {
                        Console.Clear();
                        Console.WriteLine("The customer number you searched for wasn't found.");
                    }
                }
            }

        }
    }
}