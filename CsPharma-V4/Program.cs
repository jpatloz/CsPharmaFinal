using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CsPharma_V4.Areas.Identity.Data;
using CsPharma_V4.Core.Repositorios;
using CsPharma_V4.Core.Impl;

var builder = WebApplication.CreateBuilder(args);

// Añadir servicios de razor y controladores
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

//Añade el Framework NPGSQL y el contexto para conectarnos a sus datos a través de la cadena de conexión
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<CsPharmaV4Context>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("EFCConexion"));
    });

//Añade el Framework NPGSQL y el contexto para conectarnos a sus datos a través de la cadena de conexión
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<LoginContexto>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("EFCConexion"));
    });

//activa los roles
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>()

  .AddEntityFrameworkStores<LoginContexto>();


//Inyectamos en el contenedor de dependencias estos tres servicios a través de addScoped para crear instancias de los servicios
AddScoped();

void AddScoped()
{
    builder.Services.AddScoped<UsuarioRepository, UsuarioImpl>();
    builder.Services.AddScoped<RolRepository, RolImpl>();
    builder.Services.AddScoped<WorkRepository, WorkImpl>();
}


var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
