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

        // GET: Giri� sayfas�n� g�ster
        
        [HttpGet]
        public IActionResult Login()
        {
            return View("pages-login"); // Giri� sayfas�n� d�nd�r
        }

        // POST: Kullan�c� giri�i yap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Username, string Password)
        {
            // Admin kontrol�
            var admin = _context.Admins
                .FirstOrDefault(a => a.Username == Username && a.Password == Password);
            if (admin != null)
            {
                HttpContext.Session.SetString("UserType", "Admin");
                return RedirectToAction("Index");
            }

            // Fak�lte kontrol�
            var faculty = _context.Fakulteler
                .FirstOrDefault(f => f.FakulteMail == Username && f.FakulteSifre == Password);
            if (faculty != null)
            {
                HttpContext.Session.SetString("UserType", "Fakulte");
                return RedirectToAction("Index");
            }

            // B�l�m kontrol�
            var department = _context.Bolumler
                .FirstOrDefault(d => d.BolumMail == Username && d.BolumSifre == Password);
            if (department != null)
            {
                HttpContext.Session.SetString("UserType", "Bolum");
                return RedirectToAction("Index");
            }

            // Kullan�c� bulunamad�
            ViewBag.ErrorMessage = "Kullanici adi veya sifre hatali.";
            return View("pages-login");
        }
        public IActionResult Logout()
        {
            // Oturumu temizle
            HttpContext.Session.Clear();

            // Giri� sayfas�na y�nlendir
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            // Oturum kontrol� yap
            if (HttpContext.Session.GetString("UserType") == null)
            {
                // E�er oturum a��lmam��sa, Login sayfas�na y�nlendir
                return RedirectToAction("Login");
            }

            var s�n�flar = _context.S�n�flar.ToList(); // S�n�f verisini al�yoruz 

            return View("index", s�n�flar); // index.cshtml
        }
        public IActionResult BolumEkle() // Anasayfa
        {
            return View("bolumekle");
        }

        public IActionResult DersEkle() 
        {
            return View("dersekleme"); 
        }

        public IActionResult Kullan�c�Kay�t()
        {
            return View("kullan�c�kay�t");
        }

        //public IActionResult S�n�fEkle()
        //{
        //    return View("s�n�fekleme");
        //}

        public IActionResult TablesData()
        {


            //var s�n�flar = _context.S�n�flar.ToList();

            //// Kontrol: S�n�flar bo� de�ilse, do�ru model g�nderildi�inden emin olal�m
            //if (s�n�flar != null && s�n�flar.Any())
            //{
            //    return View("tables-data", s�n�flar);  // Modeli do�ru �ekilde View'a g�nderiyoruz.
            //}
            //return View("index"); 

            var s�n�flar = _context.S�n�flar
        .Include(s => s.Fakulte)  // Fak�lte bilgilerini de dahil ediyoruz
        .ToList();

            if (s�n�flar != null && s�n�flar.Any())
            {
                return View("tables-data", s�n�flar); // Fak�lte bilgisiyle birlikte model g�nderiyoruz.
            }
            return View("index"); // E�er veriler bo�sa index'e y�nlendiririz
        }

        public IActionResult TablesGeneral(int s�n�fID)
        {
            var s�n�flar = _context.S�n�flar.ToList();



            return View("tables-general",s�n�flar);
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
