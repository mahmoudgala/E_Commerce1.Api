using ECommerce.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cate> Cates { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<UserOTP> UserOTPs { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Promotions> promotions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }



        //Deprecated 
        //I Let it just because customer view 
        //public ApplicationDbContext()
        //{



    }
}
