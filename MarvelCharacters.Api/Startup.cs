using System;
using HealthChecks.UI.Client;
using MarvelCharacters.Api.Models;
using MarvelCharacters.Api.Services;
using MarvelCharacters.Api.Services.Db;
using MarvelCharacters.Api.Services.Http.Marvel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;

namespace MarvelCharacters.Api
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
            services.AddCors();

            services.AddOptions();

            services.Configure<MarvelApiOptions>(opts =>
            {
                opts.PublicKey = Configuration["MarvelApi:PublicKey"];
                opts.PrivateKey = Configuration["MarvelApi:PrivateKey"];
                opts.Uri = Configuration["MarvelApi:Uri"];
            });

            services.AddHttpClient<HttpMarvelApi>();

            services.AddScoped<IMarvelHttpService>(ctx => ctx.GetRequiredService<HttpMarvelApi>());

            services.AddScoped<IMarvelDatabaseService, MongoDatabase>();

            //configuring options for MongoDb
            services.Configure<MongoDbOptions>(opts =>
            {
                opts.ConnectionString = Configuration["MongoDb:ConnectionString"];
                opts.Database = Configuration["MongoDb:Database"];
            });
            //adding MongoDb mapping
            BsonClassMap.RegisterClassMap<Character>(cm =>
            {
                cm.MapProperty(x => x.Name).SetIgnoreIfNull(true);
                cm.MapIdProperty(x => x.Id);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                    Title = "MarvelCharacters",
                    Version = "v1",
                    Description = "Marvel Characters API for case study only"
                });
            });

            AddHealthChecks(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        private void AddHealthChecks(IServiceCollection services)
        {
            using (var sp = services.BuildServiceProvider(false))
            {
                var mongoOpts = sp.GetRequiredService<IOptions<MongoDbOptions>>()
                    .Value;

                services.AddHealthChecks()
                    .AddMongoDb(mongodbConnectionString: mongoOpts.ConnectionString,
                                name: "mongo",
                                failureStatus: HealthStatus.Unhealthy);
            }

            //adding health check UI services
            services.AddHealthChecksUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            ConfigureInfraEndpoints(app, env);

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void ConfigureInfraEndpoints(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Marvel API");
            });

            //adding health check endpoint
            app.UseHealthChecks("/healthcheck");

            //adding health check point used by the UI
            app.UseHealthChecks("/healthz", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            //adding health check UI
            app.UseHealthChecksUI();
        }
    }
}
