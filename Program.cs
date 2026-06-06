using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using NewTalent.Data;
using NewTalent.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel server limits for large file uploads
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1073741824; // 1GB
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(5);
});

// Configure IIS limits for large file uploads (if using IIS)
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 1073741824; // 1GB
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Configure max request body size for video uploads
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1073741824; // 1GB
});

// DbContext register
builder.Services.AddDbContext<NewTalentDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Register NotificationService
builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();   // ✅ MUST BE HERE

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Talent}/{action=Explore}/{id?}"
);

app.Run();