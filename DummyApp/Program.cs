using DummyApp.Entities.Data;
using DummyApp.Repository.Interface;
using DummyApp.Repository.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DummyAppContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Ci")));
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<ICRUDRepository, CRUDRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IDapperRepository, DapperRepository>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<DummyAppContext>();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(cokkie =>
{
    cokkie.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    cokkie.LoginPath = "/Account/Login";
    cokkie.AccessDeniedPath = "/Account/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions
    //{
    //    SourceCodeLineCount = 5
    //};
    //app.UseDeveloperExceptionPage(developerExceptionPageOptions);
    //app.UseRouting();
    //app.UseEndpoints(endpoints =>
    //{
    //    endpoints.MapGet("/", context =>
    //    {
    //        throw new Exception("Error Occurred while processing your request");
    //        //await context.Response.WriteAsync("Hello World!");
    //    });
    //});
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
