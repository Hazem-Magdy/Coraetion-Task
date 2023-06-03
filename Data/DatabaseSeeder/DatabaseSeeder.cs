using CoraetionTask.Models;

namespace CoraetionTask.Data.DatabaseSeeder
{
    public class DatabaseSeeder
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                // Check if the database is already seeded
                if (context.Customers.Any() || context.Products.Any())
                {
                    return;

                    // Seed customers


                    var customers = new[]
                    {
                            new Customer { FullName = "Hazem Magdy", Mobile = "01009756572", Address = "123 Moder St" },
                            new Customer { FullName = "Mohamed Alaa", Mobile = "01005352171", Address = "456 Mokhtalat St" },
                            new Customer { FullName = "Mahmoud Ahmed", Mobile = "01008156562", Address = "333 Salam St" }

                        };

                    context.Customers.AddRange(customers);
                    context.SaveChanges();


                    // Seed products



                    var products = new[]
                    {
                            new Product { Name = "Pepsi", Price = 7.5m, CustomerID = customers[0].ID },
                            new Product { Name = "Chepsi", Price = 5.0m, CustomerID = customers[1].ID },

                        };

                    context.Products.AddRange(products);
                    context.SaveChanges();


                }

            }


        }
    }
}
