using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using CompTrain.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using CompTrain.Shared.Models.Wod;
using FluentValidation;
using CompTrain.Shared.Validators;
using System.Net.Http;

namespace CompTrain.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped<CustomAuthStateProvider>();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(
                provider => provider.GetRequiredService<CustomAuthStateProvider>());

            builder.Services.AddScoped<ILoginService, CustomAuthStateProvider>(
                provider => provider.GetRequiredService<CustomAuthStateProvider>());
            
            builder.Services.AddOptions();

            builder.Services.AddBlazoredLocalStorage();


            builder.Services.AddTransient<IValidator<WodRequest>, WodRequestValidator>();

            await builder.Build().RunAsync();
        }
    }
}
