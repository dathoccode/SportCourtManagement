﻿using Microsoft.EntityFrameworkCore;
using SportCourtManagement.Models; // namespace chứa QuanLySanTheThaoContext

var builder = WebApplication.CreateBuilder(args);

// ========== 1️⃣ Kết nối DATABASE ==========
builder.Services.AddDbContext<QuanLySanTheThaoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========== 2️⃣ Add MVC + Session ==========
builder.Services.AddControllersWithViews();

// Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tự động hết hạn sau 30 phút
    options.Cookie.HttpOnly = true;                 // Cookie không thể truy cập từ client-side script
    options.Cookie.IsEssential = true;              // Cần thiết cho ứng dụng
});

// Để sử dụng HttpContext trong View/Layout
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ========== 3️⃣ Middleware pipeline ==========
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // ⚠️ Session phải nằm TRƯỚC Authorization
app.UseAuthorization();

// ========== 4️⃣ Cấu hình Route mặc định ==========
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
