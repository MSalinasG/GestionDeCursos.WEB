using GestionDeCursos.Data.Database;
using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using GestionDeCursos.Data.Services;
using GestionDeCursos.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Database Connection
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

//Usar Dapper ORM
builder.Services.AddScoped<IDbConnection>
    (sp => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

//Conexion con el servidor mongo
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();
//Agregar servicios de las colecciones de la tabla en mongo
builder.Services.AddScoped<IExcelFileServices, ExcelFileServices>();
builder.Services.AddScoped<IPdfFileServices, PdfFileServices>();

// Add Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBreadcrumbService, BreadcrumbService>();
builder.Services.AddSingleton(typeof(IPasswordHasher<AppUser>), typeof(PasswordHasher<AppUser>));

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Manejar archivos excel EPPlus
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//Comandos a correr en la consola de Nuget
// dotnet tool install --global dotnet-sql-cache
// dotnet sql-cache create "Server=(local);Initial Catalog=BDCursos;Integrated Security=True;TrustServerCertificate=True;" dbo SqlCacheItems

builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = "dbo";
    options.TableName = "SqlCacheItems";
});

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".GestionDeCursos.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

// Datos Semilla
if (GlobalHelper.General.UseSeeder)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        DatabaseInitializer.Initialize(services);
    }
    catch (Exception ex)
    {
        var errorMessage = ex.Message;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
