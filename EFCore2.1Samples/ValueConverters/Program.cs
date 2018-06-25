using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Globalization;

namespace ValueConverters
{
    class Program
    {
        static void Main(string[] args)
        {
            //OJO
                // -NULL no se puede convertir
        }
    }

    class SomeEntity
    {
        public int Id { get; set; }

        public string ToBoolProperty { get; set; }

        public SomeEnum ToIntFromEnum { get; set; }
    }

    enum SomeEnum
    {
        Item1,
        Item2
    }

    class SomeContext
        :DbContext
    {
        public DbSet<SomeEntity> Entities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SomeEntity>()
                .Property(s => s.ToBoolProperty)
                .HasConversion<bool>(s => Convert.ToBoolean(s), b => b.ToString(CultureInfo.InvariantCulture));

            modelBuilder.Entity<SomeEntity>()
                .Property(s => s.ToIntFromEnum)
                .HasConversion(new EnumToNumberConverter<SomeEnum,int>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog = EF2.1.ValueConverters;Integrated Security=true");
            }
        }
    }

}
