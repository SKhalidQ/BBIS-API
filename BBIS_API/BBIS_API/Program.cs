using BBIS_API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;

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
            .ConfigureWebHostDefaults(webBuilder =>
            {
                Thread.Sleep(5000);
                webBuilder.UseStartup<Startup>();
            });

        private static void DebugDatabase()
        {
            using var context = new DatabaseContext();
            ProductItem Product = new ProductItem()
            {
                Brand = "Heineken",
                Flavour = "0,0",
                Alcoholic = true,
                ContainerType = "Bottle",
                Returnable = true,
                StockAmount = 125,
                SellPrice = Convert.ToDecimal(1.75),
                Discount = 0
            };
            context.ProductItems.Add(Product);
            context.SaveChanges();
        }
    }
}
