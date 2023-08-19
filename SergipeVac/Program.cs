using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SergipeVac.Infra;
using SergipeVac.Infra.Repositorios;
using SergipeVac.Model.Interface;
using SergipeVac.Servicos;
using SergipeVac;
using Microsoft.IdentityModel.Tokens;
using SergipeVac.Conversores;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));
builder.Services.AddScoped(typeof(RepositorioSincronizacao));
builder.Services.AddScoped(typeof(IServicoToken), typeof(ServicoToken));
builder.Services.AddScoped(typeof(ServicoUsuario));
builder.Services.AddScoped(typeof(ConversorDados));
builder.Services.AddScoped(typeof(ServicoSincronizacao));

builder.Services.AddControllers();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = Configuracoes.ObterChave(),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var strBuilder = new NpgsqlConnectionStringBuilder()
{
    //Port = 5432,
    //Host = "database-1.cievgxafnjws.us-east-1.rds.amazonaws.com",
    //Username = "postgres",
    //Password = "daL2n7nCHI92qGPHjfBw",
    //Database = "postgres"
    //Port = 5432,
    //Host = "100.68.8.49",
    //Username = "postgres",
    //Password = "postgres",
    //Database = "postgres"

    Port = 5432,
    Host = "localhost",
    Username = "postgres",
    Password = "vinicius11",
    Database = "SergipeVac"
};

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<Contexto>(options => options.UseNpgsql(strBuilder.ConnectionString));
builder.Services.AddHangfire(op => op.UsePostgreSqlStorage(strBuilder.ConnectionString));
builder.Services.AddHangfireServer();


using (var serviceProvider = builder.Services.BuildServiceProvider())
{
    var context = serviceProvider.GetRequiredService<Contexto>();
    context.Database.Migrate();
}

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

var sincronizador = new SincronizadorDeDados(app.Services);

RecurringJob.AddOrUpdate("Sistema de sincronização", () => sincronizador.SincronizarDados(), Cron.Monthly, TimeZoneInfo.Local);
//RecurringJob.AddOrUpdate("Teste", () => Console.WriteLine("Finalmente"), Cron.Minutely, TimeZoneInfo.Local);

app.Run();