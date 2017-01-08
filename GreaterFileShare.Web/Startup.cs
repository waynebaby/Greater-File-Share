using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.Collections.Concurrent;

namespace GreaterFileShare.Web
{
    public class Startup
    {
        public static ConcurrentDictionary<string, string> AdditionalContentTypes { get;  set; }

        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Trace);
            loggerFactory.AddProvider(new EventLogProvider());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            //app.UseStaticFiles();
            //var path = app.Properties[ Program.FileServerPathKey] as string;
            // Set up custom content types -associating file extension to MIME type
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            // Add new mappings

            if (AdditionalContentTypes!=null)
            {
                var kvps = AdditionalContentTypes.ToArray();
                foreach (var kvp in kvps)
                {
                    contentTypeProvider.Mappings[kvp.Key] = kvp.Value;
                }
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = contentTypeProvider,
                FileProvider = env.ContentRootFileProvider,
                 RequestPath="/Files"
                
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = env.ContentRootFileProvider,
                 RequestPath="/Files"
                 
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "API/{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
