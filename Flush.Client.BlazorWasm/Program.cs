using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flush.BlazorUtils;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Flush.Client.BlazorWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddLogging()
                .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
                .AddSingleton<SessionClient>()
                .AddSingleton<ChatClient>()
                .UseSessionStorage();

            await builder.Build().RunAsync();
        }
    }
}
