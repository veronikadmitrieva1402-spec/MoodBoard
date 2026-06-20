using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MoodBoardGenerator.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<MoodBoardGenerator.Client.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri("http://localhost:5259")
});

builder.Services.AddScoped<IMoodBoardApiService, MoodBoardApiService>();
builder.Services.AddScoped<IMoodBoardStateService, MoodBoardStateService>();

await builder.Build().RunAsync();