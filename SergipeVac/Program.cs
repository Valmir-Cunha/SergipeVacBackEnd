using Microsoft.EntityFrameworkCore;
using Npgsql;
using SergipeVac.Infra;
using SergipeVac.Infra.Repositorios;
using SergipeVac.Model.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var strBuilder = new NpgsqlConnectionStringBuilder()
{
    Port = 6108,
    Host = "containers-us-west-27.railway.app",
    Username = "postgres",
    Password = "sLPQxTAbuRBR60AYkT8Z",
    Database = "railway"
};


builder.Services.AddEntityFrameworkNpgsql().AddDbContext<Contexto>(options =>
options.UseNpgsql(strBuilder.ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
