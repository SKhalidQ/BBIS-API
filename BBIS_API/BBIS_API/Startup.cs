using System;
using System.Linq;
using System.Threading.Tasks;
using BBIS_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace BBIS_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase("ProductList"));
            services.AddHealthChecks();
            services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("localDatabase")));
            services.AddControllers();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = 
                Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            var mappingConfig = new MapperConfiguration(x => x.AddProfile(new MappingProfile()));
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { 
                    Title = "BBIS_API",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BBISA v1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/healthCheck");

            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
