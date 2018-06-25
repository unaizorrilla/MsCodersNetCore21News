using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Proxies
{
    class Program
    {
        static void Main(string[] args)
        {
            //install-package Microsoft.EntityFrameworkCore.SqlServer
            //install-package Microsoft.EntityFrameworkCore.Proxies

            using (var context = new SampleContext())
            {
                var blogs = context.Blogs;

                foreach (var item in blogs)
                {
                    var posts = item.Posts;
                }
            }
           
        }
    }

    class SampleContext
        :DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Integrated Security=true;Initial Catalog=EF2.1");
                optionsBuilder.UseLazyLoadingProxies(useLazyLoadingProxies: true);

                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlobConfigurer());
        }
    }

    public class Blog
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

    }

    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string  Body { get; set; }
    }

    public class BlobConfigurer
        : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasData(new Blog[]
            {
                new Blog(){Id = 100,Title ="Blog#1"},
                new Blog(){Id = 200,Title ="Blog#2"},
            });
        }
    }
}

