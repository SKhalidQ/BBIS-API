using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBIS_API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BBIS_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var ctx = new ProductContext())
            {
                ProductItem Product = new ProductItem()
                {
                    Brand = "Heineken",
                    Flavour = "0,0",
                    Alcoholic = true,
                    ContainerType = "Bottle",
                    Returnable = true,
                    StockAmount = 125,
                    SellPrice = 1.75,
                    Discount = 0
                };
                ctx.ProductItems.Add(Product);
                ctx.SaveChanges();
            }


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
