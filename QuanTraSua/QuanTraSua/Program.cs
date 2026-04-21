using Microsoft.EntityFrameworkCore;
using QuanTraSua.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Database SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Cấu hình Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 3. Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); 
app.UseAuthorization();

// 4. CẤU HÌNH ROUTE FIXED - Support /Admin/Login

// Route cho Area (Admin) 
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{action=Index}/{id?}",
    new { controller = "Admin" });

// Route cho Trang chủ (Khách hàng)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

app.Run();

