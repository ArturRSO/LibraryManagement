using LibraryManagement.Data.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Data
{
    public class DbInitializer
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            LibraryDBContext context = applicationBuilder.ApplicationServices.GetRequiredService<LibraryDBContext>();

            UserManager<IdentityUser> userManager = applicationBuilder.ApplicationServices.GetRequiredService<UserManager<IdentityUser>>();

            // Add Lender
            var user = new IdentityUser("Artur Rios");
            await userManager.CreateAsync(user, "%Artur");

            // Add Customers
            var c1 = new Customer { Name = "Peter Parker" };
            var c2 = new Customer { Name = "Jack White" };
            var c3 = new Customer { Name = "Bruce Banner" };

            context.Customers.Add(c1);
            context.Customers.Add(c2);
            context.Customers.Add(c3);

            // Add Authors
            var a1 = new Author
            {
                Name = "Philip K. Dick",
                Books = new List<Book>()
                {
                    new Book { Title = "Do Androids Dream of Electric Sheep?" },
                    new Book { Title = "A Scanner Darkly" },
                    new Book { Title = "Flow My Tears, The Policeman Said" }
                }
            };

            var a2 = new Author
            {
                Name = "Jules Verne",
                Books = new List<Book>()
                {
                    new Book { Title = "Twenty Thousand Leagues Under the Sea" },
                    new Book { Title = "Journey to the Center of the Earth" }
                }
            };

            context.Authors.Add(a1);
            context.Authors.Add(a2);

            context.SaveChanges();
        }
    }
}
