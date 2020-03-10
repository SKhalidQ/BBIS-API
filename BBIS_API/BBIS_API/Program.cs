using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;
using BBIS_API.Models;
using System.Linq;
using System;

namespace BBIS_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Force Enter Data into Table

            //DebugDatabase();

            #endregion

            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>{
                webBuilder.UseStartup<Startup>();
            });

        public static void DebugDatabase()
        {
            using (var context = new DatabaseContext())
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
                context.ProductItems.Add(Product);
                context.SaveChanges();
            }
        }
    }
}
