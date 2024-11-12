using DersPrgProjesi.Data; // DbContext'i eklemek için
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection string’i appsettings.json’dan alýn
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// DbContext'i ekleyin
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(@"Data Source=DESKTOP-CB1H4N6\SQLEXPRESS;Initial Catalog=DersProgramiSitesi;Integrated Security=True;Trust Server Certificate=True"));

// ASP.NET Core Identity'i ekleyin
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Diðer servisleri ekleyin
builder.Services.AddControllersWithViews();
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Geliþtirme ortamýnda hata ayýklamak için


builder.Logging.ClearProviders();
builder.Logging.AddConsole();  // Konsol loglamayý ekleyin


var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}"); // Varsayýlan rotayý ayarla
app.MapRazorPages();

app.Run(); // Uygulamayý çalýþtýr
