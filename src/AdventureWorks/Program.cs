using AdventureWorks.DataAccess;
using AdventureWorks.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Initialize SQLite Batteries to load native libraries
SQLitePCL.Batteries_V2.Init();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<UserService>();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton(provider =>
{
    var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
    var connection = new SqliteConnection("Filename=:memory:");
    connection.Open();
    builder.UseSqlite(connection)
        .UseLoggerFactory(provider.GetRequiredService<ILoggerFactory>());
    return builder.Options;
});

builder.Services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Transient);

var app = builder.Build();

using (var context = app.Services.GetRequiredService<ApplicationDbContext>())
{
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
