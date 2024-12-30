using Cliente.Aplicacion.Repositorios;
using Cliente.Infraestructura.Data;
using Cliente.Infraestructura.Repositorios;
using Clientes.Aplicacion.Servicios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Registrar DbContext

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ClienteDbContext>(options =>
    options.UseSqlServer(connectionString));

//Registrar Repositorios
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();

//Registrar servicios
builder.Services.AddScoped<ClienteServicio>();

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
