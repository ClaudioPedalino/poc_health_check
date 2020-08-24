using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using poc_health_check.Data;
using poc_health_check.FileChecker;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace poc_health_check
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
            #region db
            services.AddDbContext<DataContext>(options =>
                //options.UseNpgsql(Configuration.GetConnectionString("PostgreSql")))
                options.UseSqlServer(Configuration.GetConnectionString("Sql")));
            services.AddControllers();
            #endregion

            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("Sql"),
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "ready" })
                .AddUrlGroup(new Uri(Configuration.GetValue<string>("BookApi")),
                    "Dummy Book-Data Health Check",
                    HealthStatus.Degraded,
                    timeout: new TimeSpan(0, 0, 5),
                    tags: new[] { "ready" })
                .AddCheck("File Path Health Check", new FilePathWriteHealthCheck(Configuration["DummyFilePath"]),
                    HealthStatus.Unhealthy,
                    tags: new[] { "ready" });

            services.AddHealthChecksUI().AddInMemoryStorage();

            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PoC Health Check", Version = "v1" });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region default
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("api/health/ready", new HealthCheckOptions()
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    },
                    ResponseWriter = WriteHealthCheckReadyResponse,
                    Predicate = (check) => check.Tags.Contains("ready"),
                    AllowCachingResponses = false
                });
                endpoints.MapHealthChecks("api/health/live", new HealthCheckOptions()
                {
                    Predicate = (check) => !check.Tags.Contains("ready"),
                    ResponseWriter = WriteHealthCheckLiveResponse,
                    AllowCachingResponses = false
                });
                endpoints.MapHealthChecks("api/health/ui", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            app.UseHealthChecksUI();

            #region swagger-ui
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PoC Health Check");
            });
            #endregion
        }

        
        //Enrich Health Check Detail
        private Task WriteHealthCheckLiveResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                    new JProperty("OverallStatus", result.Status.ToString()),
                    new JProperty("TotalCheckDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00"))
                );
            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }

        private Task WriteHealthCheckReadyResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                    new JProperty("OverallStatus", result.Status.ToString()),
                    new JProperty("TotalCheckDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00")),
                    new JProperty("DependencyHealthChecks", new JObject(result.Entries.Select(dicItem =>
                        new JProperty(dicItem.Key, new JObject(
                            new JProperty("Status", dicItem.Value.Status.ToString()),
                            new JProperty("Duration", dicItem.Value.Duration.TotalSeconds.ToString("0:0.00")),
                            new JProperty("Exception", dicItem.Value.Exception?.Message),
                            new JProperty("Data", new JObject(dicItem.Value.Data.Select(dicData =>
                                new JProperty(dicData.Key, dicData.Value))))
                        ))))
                ));
            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }

    }
}
