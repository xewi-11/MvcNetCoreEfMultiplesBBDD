using Microsoft.EntityFrameworkCore;
using MvcNetCoreEfMultiplesBBDD.Data;
using MvcNetCoreEfMultiplesBBDD.Repositories;

var builder = WebApplication.CreateBuilder(args);

//-------------------------------------------Oracle------------------------------------------------------------
builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosOracle>();
//builder.Services.AddDbContext<HospitalContext>(options => options.UseOracle(builder.Configuration.GetConnectionString("oracleHospital")));


//-------------------------------------------SqlServer------------------------------------------------------------
builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosSqlServer>();

//builder.Services.AddDbContext<HospitalContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlHospital")));

//-------------------------------------------MySql------------------------------------------------------------
builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosMySql>();
builder.Services.AddDbContext<HospitalContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("MySqlHospital")));

// Add services to the container.


builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
