using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;

namespace GreaterFileShare.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(args.FirstOrDefault() ?? Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();

        }

        public static async Task StartAsync(string folderName, int port = 8080, CancellationToken cancellationToken = default(CancellationToken))
        {
            var host = new WebHostBuilder()
              .UseKestrel()
              .UseContentRoot(folderName ?? Directory.GetCurrentDirectory())
              .UseIISIntegration()
              .UseStartup<Startup>()
              .Build();
            await RunAsync(host, cancellationToken, "");
        }

        private static async Task RunAsync(IWebHost host, CancellationToken token, string shutdownMessage)
        {
            using (host)
            {
                host.Start();
                var tcs = new TaskCompletionSource<object>();
                var hostingEnvironment = host.Services.GetService<IHostingEnvironment>();
                var applicationLifetime = host.Services.GetService<IApplicationLifetime>();

                Console.WriteLine($"Hosting environment: {hostingEnvironment.EnvironmentName}");
                Console.WriteLine($"Content root path: {hostingEnvironment.ContentRootPath}");

                var serverAddresses = host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
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
