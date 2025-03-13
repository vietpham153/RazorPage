using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RazorPage.Models
{
    public class MyBlogContext : IdentityDbContext<AppUser>
    {
        public DbSet<Article> articles { get; set; }
        public MyBlogContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
