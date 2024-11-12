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
        public IActionResult Login(admin ad)
        {
            
            var admin = _context.Admins
                .FirstOrDefault(a => a.Username == ad.Username && a.Password == ad.Password);

            if (admin != null)
            {
               
                return RedirectToAction("index");
            }
            else
            {
                
                return View("pages-login");
            }

        }

        public IActionResult Index() // Anasayfa
        {
            return View("index"); // index.cshtml
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

        public IActionResult SýnýfEkle()
        {
            return View("sýnýfekleme");
        }

        public IActionResult TablesData()
        {
            return View("tables-data");
        }

        public IActionResult TablesGeneral()
        {
            return View("tables-general");
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
