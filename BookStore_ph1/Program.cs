using BookStore_ph1.Data;
using BookStore_ph1.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    // Configure password settings if needed
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
});

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var RoleManager = 
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User", "Guest" };

    foreach (var role in roles)
    {
        if(!await RoleManager.RoleExistsAsync(role))
        {
            await RoleManager.CreateAsync(new IdentityRole(role));
        }
    }

}


using (var scope = app.Services.CreateScope())
{
    var UserManager =
        scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    string email = "admin@bookstore.com";
    string password = "Password1!";


    if(await UserManager.FindByEmailAsync(email) == null)
    {
        var User = new User();
        User.UserName = email;
        User.Email = email;
        await UserManager.CreateAsync(User, password);
        User.Name = email.Split("@")[0];
        User.Role = "Admin";
        await UserManager.AddToRoleAsync(User, "Admin");
    }

}

app.Run();
