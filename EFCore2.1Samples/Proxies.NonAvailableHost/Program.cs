using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace Proxies.NonAvailableHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //install-package Microsoft.EntityFrameworkCore.SqlServer
            //install-package Microsoft.EntityFrameworkCore.Abstractions

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
        : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Integrated Security=true;Initial Catalog=EF2.1");
            }
        }
    }

    class Blog
    {
        public int Id { get; set; }

        public string Title { get; set; }

        private ICollection<Post> _posts;

        public ICollection<Post> Posts
        {
            get
            {
                return _loader.Load(this, ref _posts);
            }
            set
            {
                _posts = value;
            }
        }

        private readonly ILazyLoader _loader;

        public Blog(ILazyLoader loader)
        {
            _loader = loader;
        }

    }

    class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }

}
