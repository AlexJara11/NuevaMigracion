using BackSeccion1.Services.Alumnos;
using BackSeccion1.Services.Cursos;
using BackSeccion1.Services.Secciones;
using FrontSeccion;
using FrontSeccion.Services.Alumnos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

builder.Services.AddScoped<AlumnoServiceCrear>();
builder.Services.AddScoped<AlumnoServiceUpdate>();
builder.Services.AddScoped<AlumnoServiceDelete>();

builder.Services.AddScoped<CursoServiceCrear>();
builder.Services.AddScoped<CursoServiceUpdate>();
builder.Services.AddScoped<CursoServiceDelete>();

builder.Services.AddScoped<SeccionServicesCrear>();
builder.Services.AddScoped<SeccionServicesUpdate>();
builder.Services.AddScoped<SeccionServicesDelete>();


var app = builder.Build();


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "ExportarPdf",
//        pattern: "Seccion/ExportarAlumnosAPdf/{idSec}",
//        defaults: new { controller = "Seccion", action = "ExportarAlumnosAPdf" }
//    );

//    // Otras rutas de tu aplicación...

//    endpoints.MapControllerRoute(
//        name: "Default",
//        pattern: "{controller=Home}/{action=Index}/{id?}"
//    );
//});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
