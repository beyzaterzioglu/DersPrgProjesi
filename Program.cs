using DersPrgProjesi.Data; // DbContext'i eklemek için
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext'i ekleyin
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(@"Data Source=DESKTOP-CB1H4N6\SQLEXPRESS;Initial Catalog=DersProgramiSitesi;Integrated Security=True;Trust Server Certificate=True"));

// ASP.NET Core Identity'i ekleyin
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Session servisini ekleyin
builder.Services.AddDistributedMemoryCache(); // Session için gerekli
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman aþýmý
    options.Cookie.HttpOnly = true; // Güvenlik için sadece HTTP üzerinden eriþim
    options.Cookie.IsEssential = true; // Zorunlu cookie
});

// Diðer servisleri ekleyin
builder.Services.AddControllersWithViews();
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Geliþtirme ortamýnda hata ayýklamak için

builder.Logging.ClearProviders();
builder.Logging.AddConsole();  // Konsol loglamayý ekleyin

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Session middleware burada tanýmlanmalý
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); // Migrations'larý geliþtirme ortamýnda göster
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Hata durumunda yönlendirme
    app.UseHsts(); // HSTS kullan
}

app.Run(); // Uygulamayý çalýþtýr
