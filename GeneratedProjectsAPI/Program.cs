
using Figgle;

Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine(FiggleFonts.Varsity.Render("     W E L C O M E    "));
Console.Write(FiggleFonts.Banner3D.Render("     <0 v 0>    "));


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
