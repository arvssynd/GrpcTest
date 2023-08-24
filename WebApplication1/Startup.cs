using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.ComponentModel;
using System.Globalization;
using System;

namespace WebApplication1;

public class Startup
{    
    public Startup()
    {
        
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();

        // in the ConfigureServices()
        services.AddControllers(config =>
        {
            //config.Filters.Add(typeof(CustomExceptionFilterAttribute));
            //config.Filters.Add(typeof(CustomActionFilterAttribute));
        });
       
        // cors
        //services.AddCors(options =>
        //{
        //    options.AddPolicy("CorsPolicy",
        //        builder =>

        //        builder.WithOrigins(_generalConfig.GetValue<string>(nameof(GeneralConfig.CorsAllowOrigins)).Split(","))
        //        .AllowAnyMethod()
        //        .AllowAnyHeader()
        //        .AllowCredentials());
        //});

        //DI
        //Remark --> (Scoped lifetime services are created once per client request)
        //           (Transient lifetime services are created each time they're requested from the service container)
        services.AddMemoryCache();

        //var configMapper = new MapperConfiguration(cfg =>
        //{
        //    cfg.AddProfile(new AutoMapperProfile());
        //});
        //IMapper mapper = configMapper.CreateMapper();
        //services.AddSingleton(mapper);

        //services.AddScoped<IUserLogged, UserLogged>();

        //services.AddRepositories();
        //services.AddValidators();
        //services.AddServices();

        //// configurations
        //services.Configure<JwtConfig>(AppSettings.GetSection(AppSettingsSections.Jwt));
        //services.Configure<MailConfig>(AppSettings.GetSection(AppSettingsSections.Mail));
        //services.Configure<RootConfig>(AppSettings.GetSection(AppSettingsSections.Root));
        //services.Configure<CacheConfig>(AppSettings.GetSection(AppSettingsSections.Cache));
        //services.Configure<ConnectionStringsConfig>(AppSettings.GetSection(AppSettingsSections.ConnectionStrings));
        //services.Configure<ApplicationInsightsConfig>(AppSettings.GetSection(AppSettingsSections.ApplicationInsights));
        //services.Configure<GeneralConfig>(AppSettings.GetSection(AppSettingsSections.General));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCors("CorsPolicy");

            //app.UseSwagger();
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nival.NivalService v1"));
        }

        //middleware
        //if (_applicationInsightsConfig.GetValue<bool>(nameof(ApplicationInsightsConfig.ApplicationInsightsEnabled)))
        //{
        //    app.UseRequestBodyLogging();
        //    app.UseResponseBodyLogging();
        //}

        app.Use(async (context, next) =>
        {
            await next();
            if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
            {
                context.Request.Path = "/index.html";
                await next();
            }
        })
       .UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } })
       .UseStaticFiles();

        //dbInitializerService.Initialize().Wait();

        // culture definition, da inserire prima di use routing
        var ci = new CultureInfo("it-IT");
        ci.NumberFormat.NumberDecimalSeparator = ",";
        ci.NumberFormat.NumberGroupSeparator = ".";
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(ci),
            SupportedCultures = new List<CultureInfo>
        {
            ci,
        },
            SupportedUICultures = new List<CultureInfo>
        {
            ci,
        }
        });

        app.UseRouting();

        // Use Authentication deve essere dopo il useRouting
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
            name: "default",
            pattern: "api/{controller}/{action}/{id?}");
        });
    }
}
