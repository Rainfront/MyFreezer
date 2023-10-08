using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NET6.BlazorWebAssemblyApp;
using BlazorBootstrap;
using BlazorStrap;
using MyFreezer.API.DataServices; // Add this line

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7023/") });

builder.Services.AddBlazorBootstrap(); // Add this line
builder.Services.AddBlazorStrap();

builder.Services.AddHttpClient<IUserDataService, UserDataService>(
    //spds => spds.BaseAddress = new Uri(builder.Configuration["api_base_url"]));
    spds => spds.BaseAddress = new Uri("https://localhost:7023/"));
/*Console.WriteLine("------------------------");
Console.WriteLine(builder.Configuration["api_base_url"]);
Console.WriteLine("------------------------");*/

await builder.Build().RunAsync();
