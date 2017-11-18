using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartPlayerAPI.Persistance;
using Microsoft.EntityFrameworkCore;
using SmartPlayerAPI.Repository.Persistence;
using SmartPlayerAPI.Repository.Interfaces;
using AutoMapper;
using System.Reflection;
using SmartPlayerAPI.ViewModels.Modules;
using SmartPlayerAPI.Persistance.Models;
using SmartPlayerAPI.ViewModels.Sensors.GPS;

namespace SmartPlayerAPI
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

            //Register swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "SmartPlayer API 1.0", Version = "v1" });
            });

            services.AddAutoMapper(ctx =>
            {
                ctx.CreateMap<Persistance.Models.Module, ModuleOut>()
                .ForMember(i => i.MACAddress, o => o.MapFrom(i => i.MACAddress))
                .ForMember(i => i.Id, o => o.MapFrom(i => i.Id));
       
                ctx.CreateMap<List<Persistance.Models.Module>, List<ModuleOut>>();
                ctx.CreateMap<List<ModuleOut>, List<Persistance.Models.Module>>();
                ctx.CreateMap<GPSLocation, PointInTime>();
                ctx.CreateMap<List<Persistance.Models.GPSLocation>, List<PointInTime>>();

            }, assemblies: Enumerable.Empty<Assembly>());
            //Configure db
            services.AddDbContextPool<SmartPlayerContext>(o =>
            {
                o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IAccelerometerAndGyroscopeRepository, AccelerometerAndGyroscopeRepository>();
            services.AddScoped<IGPSLocationRepository, GPSLocationRepository>();
            services.AddScoped<IPlayerInGameRepository, PlayerInGameRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartPlayer API V1");
            });
            app.UseMvc();
        }
    }
}
