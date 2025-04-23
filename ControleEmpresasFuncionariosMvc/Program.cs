using ControleEmpresasFuncionariosMvc.Data;
using ControleEmpresasFuncionariosMvc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*
 * Ordem de construção:
 * 1. Controller (apenas a parte que aciona views)
 * 2. View (Para que antes de construir o backend o front seja aprovado)
 * 3. Controller (finalmente a parte que aciona as APIs)
 * 4. Service
 * 5. Model
 */

builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<JobService>();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<JobsPersonsService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ControleEmpresasFuncionariosMvcContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        options => { options.CommandTimeout(120); }
        );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/NotFound");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
