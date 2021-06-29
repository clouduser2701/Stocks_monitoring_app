using Blazor.Extensions.Storage;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StocksApp.Client.Services;
using Syncfusion.Blazor;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StocksApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDYxMjIxQDMxMzkyZTMxMmUzMEJOKzg1WkNOaFppeTJabnlSTFhEODZvU2JXWkpJZnp5QUVQcTc5V2dkeUU9");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddTransient<IApiHttpService, ApiHttpService>();

            builder.Services.AddHttpClient("StocksApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("StocksApp.ServerAPI"));

            builder.Services.AddApiAuthorization(options =>
            {

                options.AuthenticationPaths.LogOutSucceededPath = "authentication/login";

            });
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddApiAuthorization();

            builder.Services.AddStorage();
            

            await builder.Build().RunAsync();
        }
    }
}
