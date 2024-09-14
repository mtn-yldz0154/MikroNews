using Microsoft.EntityFrameworkCore;
using MikroNews.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikroNews.DataAccessLayer.Context
{
    public class NewsContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=LAPTOP-LM2N83TK;database=MikroNewsDB4;" +
                "integrated security=true;");
        }


        public DbSet<News> News { get; set; }
        public DbSet<Logs> Logs { get; set; }
    }
}
