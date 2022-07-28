using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

    builder.Services.AddCors(options =>
        options.AddDefaultPolicy(
            builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
    );




var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseCors();
}





// Configure the HTTP request pipeline.

app.UseAuthorization();
app.MapControllers();
app.Run();
