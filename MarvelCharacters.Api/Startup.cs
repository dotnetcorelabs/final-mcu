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
using MongoDB.Bson.Serialization.IdGenerators;

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
