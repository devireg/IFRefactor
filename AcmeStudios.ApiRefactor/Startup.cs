using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using AcemStudios.ApiRefactor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace AcemStudios.ApiRefactor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Cont>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("StudioConnection"));
            });
            services.AddCors(options =>
             {
                 options.AddPolicy("AllowMyOrigin",
                 builder => builder.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader());
             });

            services.AddControllers();

            //services.AddSwaggerGen();

            //Updating the swagger with API version -Devi
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("AllowMyOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllers().RequireCors("AllowMyOrigin");
            });
            //UseSwagger() -Devi
            app.UseSwagger();
            //UseSwaggerUI -Devi
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            });
        }
    }
}
