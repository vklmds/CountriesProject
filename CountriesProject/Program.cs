using CountriesProject;
using CountriesProject.Interfaces;
using CountriesProject.Repository;
using CountriesProject.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbConnection")));

builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<ICountryRepository,CountryRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.MapRazorPages();

app.Run();
