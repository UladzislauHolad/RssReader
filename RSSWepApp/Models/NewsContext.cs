﻿using System.Data.Entity;


namespace RSSWepApp.Models
{
    class NewsContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().HasKey(a => new { a.Title, a.PublishDate });
        }
    }
}