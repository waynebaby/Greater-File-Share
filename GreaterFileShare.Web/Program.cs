using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;

namespace GreaterFileShare.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();

        }
        public const string FileServerPathKey = nameof(FileServerPathKey);
        public const string FileServerContentTypesKey = nameof(FileServerContentTypesKey);

        public static async Task StartAsync(string folderName, int port = 8080,IDictionary<string,string> extentionContentTypeDict =null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (extentionContentTypeDict!=null)
            {
                Startup.AdditionalContentTypes = new System.Collections.Concurrent.ConcurrentDictionary<string, string>(extentionContentTypeDict);
            }
            var contentDir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.GetDirectories("contents").FirstOrDefault();
            var host = new WebHostBuilder()
                .UseKestrel()              
                .UseContentRoot(folderName)
                .UseUrls("http://0.0.0.0:" + port.ToString())
                .UseStartup<Startup>()
                .Build();
            await RunAsync(host, cancellationToken, "", port);
        }

        private static async Task RunAsync(IWebHost host, CancellationToken token, string shutdownMessage, int port = 8080)
        {
            using (host)
            {
                host.Start();
                var tcs = new TaskCompletionSource<object>();
                var hostingEnvironment = host.Services.GetService<IHostingEnvironment>();
                var applicationLifetime = host.Services.GetService<IApplicationLifetime>();

                Console.WriteLine($"Hosting environment: {hostingEnvironment.EnvironmentName}");
                Console.WriteLine($"Content root path: {hostingEnvironment.ContentRootPath}");

                var serverAddresses = new UriBuilder() { Host = "localhost", Port = port, Scheme = "http" }.ToString();  //host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
                if (serverAddresses != null)
                {
                    foreach (var address in serverAddresses)
                    {
                        Console.WriteLine($"Now listening on: {address}");
                    }
                }

                if (!string.IsNullOrEmpty(shutdownMessage))
                {
                    Console.WriteLine(shutdownMessage);
                }

                token.Register(() =>
                {
                    applicationLifetime.StopApplication();
                });

                applicationLifetime.ApplicationStopping.Register(() =>
                {
                    tcs.TrySetCanceled();
                });

                await tcs.Task;
            }
        }

    }
}
