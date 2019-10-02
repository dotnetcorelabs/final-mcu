using MarvelCharacters.Api.Models;
using MarvelCharacters.Api.Services;
using MarvelCharacters.Api.Services.Db;
using MarvelCharacters.Api.Services.Http.Marvel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
