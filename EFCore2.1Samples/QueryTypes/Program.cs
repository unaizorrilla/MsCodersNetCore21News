using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SampleDbContext())
            {
                //this only work if the query materialize all  properties on the entity
                var customers = context.Customer
                    .FromSql("SELECT Id,FirstName FROM dbo.Customer")
                    .ToList();

                //error:The required column 'Id' was not present in the results of a 'FromSql' operation
                var customersFirstNames = context.Customer
                    .FromSql("SELECT FirstName from dbo.Customer c inner join dbo.Orders o on c.Id=o.CustomerId WHERE o.Total > 1000")
                    .ToList();


                var customersWithQuery = context.PrevalentCustomerQuery
                    .ToList();

            }
        }
    }

    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }

        public decimal Total { get; set; }
    }

    public class MyQueryResult
    {
        public string FirstName { get; set; }
    }

    public class SampleDbContext
        :DbContext
    {
        public DbSet<Customer> Customer { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbQuery<MyQueryResult> PrevalentCustomerQuery { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Mapping to ToView
            modelBuilder.Query<MyQueryResult>()
                .ToQuery(() => Customer.Select(c => new MyQueryResult() { FirstName = c.FirstName }));

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog = EF2.1.QueryTypes;Integrated Security=true");
            }
        }
    }
}
