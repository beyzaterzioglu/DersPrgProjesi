using DersPrgProjesi.Data; // DbContext'i eklemek i�in
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
builder.Services.AddDistributedMemoryCache(); // Session i�in gerekli
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum zaman a��m�
    options.Cookie.HttpOnly = true; // G�venlik i�in sadece HTTP �zerinden eri�im
    options.Cookie.IsEssential = true; // Zorunlu cookie
});

// Di�er servisleri ekleyin
builder.Services.AddControllersWithViews();
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Geli�tirme ortam�nda hata ay�klamak i�in

builder.Logging.ClearProviders();
builder.Logging.AddConsole();  // Konsol loglamay� ekleyin

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Session middleware burada tan�mlanmal�
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
    app.UseMigrationsEndPoint(); // Migrations'lar� geli�tirme ortam�nda g�ster
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Hata durumunda y�nlendirme
    app.UseHsts(); // HSTS kullan
}

app.Run(); // Uygulamay� �al��t�r
