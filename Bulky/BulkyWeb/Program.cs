using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories;
using Bulky.DataAccess.Repositories.Innterfaces;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var conStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("Bulky_CON_COM"));
conStrBuilder.Password = builder.Configuration["Bulky_Pass"];
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(conStrBuilder.ConnectionString));
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()//(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = $"/Identity/Account/Login";
    opt.LogoutPath = $"/Identity/Account/Logout";
    opt.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
