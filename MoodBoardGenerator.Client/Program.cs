using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MoodBoardGenerator.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri("https://localhost:5001")
});

builder.Services.AddScoped<IMoodBoardApiService, MoodBoardApiService>();
builder.Services.AddScoped<IMoodBoardStateService, MoodBoardStateService>();

await builder.Build().RunAsync();