using Microsoft.OpenApi.Models;
using MoodBoardGenerator.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MoodBoard Generator API",
        Version = "v1",
        Description = "API for generating and managing mood boards"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddSingleton<IMoodBoardService, MoodBoardService>();
builder.Services.AddSingleton<IMoodBoardRepository, InMemoryMoodBoardRepository>();
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazor");
app.UseAuthorization();
app.MapControllers();

app.Run();