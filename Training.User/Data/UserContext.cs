using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Data
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<RegisterEntity> Registers { get; set; }
      


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisterEntity>().HasData(
                new RegisterEntity
                {
                    RegisterId = 6,
                    Email = "adminFirst@gmail.com",
                    Password = "admin@123456",
                    Phone = 2345678905,
                    isAdmin = true,
                    Name = "Admin2"
                },
                new RegisterEntity
                {
                    RegisterId = 7,
                    Email = "hariomroy@gmail.com",
                    Password = "admin@123457",
                    Phone = 1234567896,
                    isAdmin = true,
                    Name = "Anshu"
                }, new RegisterEntity
                {
                    RegisterId = 8,
                    Email = "admin34@gmail.com",
                    Password = "admin@123455",
                    Phone = 12345678960,
                    isAdmin = true,
                    Name = "PriyamAdmin"
                }
                );
        }
    }
}
