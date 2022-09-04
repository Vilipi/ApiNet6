global using ApiNet6.Data;
global using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.OpenApi.Models; // para la documentación de Swagger

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Add Entity Framework services to the services container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    // Modificando addSwaggergen para poder poner nuestros comentarios

    c => {
        c.EnableAnnotations();
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "AA2 vicente Lizandra", Version = "v1" });

    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 

app.UseSwagger();
app.UseSwaggerUI((options) => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseCors(options => {
    options.AllowAnyMethod();
    options.AllowAnyHeader();

});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

