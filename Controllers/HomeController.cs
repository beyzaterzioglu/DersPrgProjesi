using DersPrgProjesi.Data;
using DersPrgProjesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DersPrgProjesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; // DbContext nesnesi
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Giriþ sayfasýný göster
        
        [HttpGet]
        public IActionResult Login()
        {
            return View("pages-login"); // Giriþ sayfasýný döndür
        }

        // POST: Kullanýcý giriþi yap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Username, string Password)
        {
            // Admin kontrolü
            var admin = _context.Admins
                .FirstOrDefault(a => a.Username == Username && a.Password == Password);
            if (admin != null)
            {
                HttpContext.Session.SetString("UserType", "Admin");
                return RedirectToAction("Index");
            }

            // Fakülte kontrolü
            var faculty = _context.Fakulteler
                .FirstOrDefault(f => f.FakulteMail == Username && f.FakulteSifre == Password);
            if (faculty != null)
            {
                HttpContext.Session.SetString("UserType", "Fakulte");
                return RedirectToAction("Index");
            }

            // Bölüm kontrolü
            var department = _context.Bolumler
                .FirstOrDefault(d => d.BolumMail == Username && d.BolumSifre == Password);
            if (department != null)
            {
                HttpContext.Session.SetString("UserType", "Bolum");
                return RedirectToAction("Index");
            }

            // Kullanýcý bulunamadý
            ViewBag.ErrorMessage = "Kullanici adi veya sifre hatali.";
            return View("pages-login");
        }
        public IActionResult Logout()
        {
            // Oturumu temizle
            HttpContext.Session.Clear();

            // Giriþ sayfasýna yönlendir
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            // Oturum kontrolü yap
            if (HttpContext.Session.GetString("UserType") == null)
            {
                // Eðer oturum açýlmamýþsa, Login sayfasýna yönlendir
                return RedirectToAction("Login");
            }

            var sýnýflar = _context.Sýnýflar.ToList(); // Sýnýf verisini alýyoruz 

            return View("index", sýnýflar); // index.cshtml
        }
        public IActionResult BolumEkle() // Anasayfa
        {
            return View("bolumekle");
        }

        public IActionResult DersEkle() 
        {
            return View("dersekleme"); 
        }

        public IActionResult KullanýcýKayýt()
        {
            return View("kullanýcýkayýt");
        }

        //public IActionResult SýnýfEkle()
        //{
        //    return View("sýnýfekleme");
        //}

        public IActionResult TablesData()
        {


            //var sýnýflar = _context.Sýnýflar.ToList();

            //// Kontrol: Sýnýflar boþ deðilse, doðru model gönderildiðinden emin olalým
            //if (sýnýflar != null && sýnýflar.Any())
            //{
            //    return View("tables-data", sýnýflar);  // Modeli doðru þekilde View'a gönderiyoruz.
            //}
            //return View("index"); 

            var sýnýflar = _context.Sýnýflar
        .Include(s => s.Fakulte)  // Fakülte bilgilerini de dahil ediyoruz
        .ToList();

            if (sýnýflar != null && sýnýflar.Any())
            {
                return View("tables-data", sýnýflar); // Fakülte bilgisiyle birlikte model gönderiyoruz.
            }
            return View("index"); // Eðer veriler boþsa index'e yönlendiririz
        }

        public IActionResult TablesGeneral(int sýnýfID)
        {
            var sýnýflar = _context.Sýnýflar.ToList();



            return View("tables-general",sýnýflar);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
