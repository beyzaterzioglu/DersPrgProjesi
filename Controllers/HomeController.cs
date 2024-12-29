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
                HttpContext.Session.SetInt32("FakulteID", faculty.FakulteID); // Fak�lte ID kaydediliyor
                return RedirectToAction("Index");
            }

            // B�l�m kontrol�
            var department = _context.Bolumler
                .FirstOrDefault(d => d.BolumMail == Username && d.BolumSifre == Password);
            if (department != null)
            {
                HttpContext.Session.SetString("UserType", "Bolum");
                HttpContext.Session.SetInt32("BolumID", department.BolumID); // B�l�m ID kaydediliyor
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
            //// Oturum kontrol� yap
            //if (HttpContext.Session.GetString("UserType") == null)
            //{
            //    // E�er oturum a��lmam��sa, Login sayfas�na y�nlendir
            //    return RedirectToAction("Login");
            //}

            //var s�n�flar = _context.S�n�flar.ToList(); // S�n�f verisini al�yoruz 

            //return View("index", s�n�flar); // index.cshtml

            // Oturum kontrol� yap
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(userType))
            {
                // E�er oturum a��lmam��sa, Login sayfas�na y�nlendir
                return RedirectToAction("Login");
            }

            // Kullan�c� t�r�ne g�re s�n�flar� filtrele
            var s�n�flar = new List<S�n�f>();

            if (userType == "Admin")
            {
                // Admin t�m s�n�flar� g�rebilir
                s�n�flar = _context.S�n�flar.ToList();
            }
            else if (userType == "Fakulte")
            {
                // Fak�lte sadece kendi fak�ltesine ait s�n�flar� g�rebilir
                var fakulteId = HttpContext.Session.GetInt32("FakulteID");
                s�n�flar = _context.S�n�flar
                    .Where(s => s.FakulteID == fakulteId) // Fak�lteID ile filtrele
                    .ToList();
            }
            else if (userType == "Bolum")
            {
                // B�l�m sadece kendi fak�ltesine ait s�n�flar� g�rebilir
                var bolumId = HttpContext.Session.GetInt32("BolumID");
                var bolum = _context.Bolumler.FirstOrDefault(b => b.BolumID == bolumId);

                if (bolum != null)
                {
                    s�n�flar = _context.S�n�flar
                        .Where(s => s.FakulteID == bolum.FakulteID) // B�l�m�n fak�lte ID'sine g�re filtrele
                        .ToList();
                }
            }

            return View("index", s�n�flar); // index.cshtml'e s�n�f listesini g�nder
        }
        public IActionResult BolumEkle() // Anasayfa
        {
            return View("bolumekle");
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

        //    var s�n�flar = _context.S�n�flar
        //.Include(s => s.Fakulte)  // Fak�lte bilgilerini de dahil ediyoruz
        //.ToList();

        //    if (s�n�flar != null && s�n�flar.Any())
        //    {
        //        return View("tables-data", s�n�flar); // Fak�lte bilgisiyle birlikte model g�nderiyoruz.
        //    }
        //    return View("index"); // E�er veriler bo�sa index'e y�nlendiririz



            // Kullan�c� tipini ve Fak�lteNo'yu oturumdan al
            var userType = HttpContext.Session.GetString("UserType");
            //var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fak�lte numaras�

            //List<S�n�f> s�n�flar;
            var s�n�flar = new List<S�n�f>();

            if (userType == "Admin")
            {
           
              s�n�flar = _context.S�n�flar.ToList();
            }
            else if (userType == "Fakulte")
            {
                var fakulteId = HttpContext.Session.GetInt32("FakulteID");
                s�n�flar = _context.S�n�flar
                    .Where(s => s.FakulteID == fakulteId) // Fak�lteID ile filtrele
                    .ToList();
            }
            else if ( userType == "Bolum")
            {
                // B�l�m sadece kendi fak�ltesine ait s�n�flar� g�rebilir
                var bolumId = HttpContext.Session.GetInt32("BolumID");
                var bolum = _context.Bolumler.FirstOrDefault(b => b.BolumID == bolumId);

                if (bolum != null)
                {
                    s�n�flar = _context.S�n�flar
                        .Where(s => s.FakulteID == bolum.FakulteID) // B�l�m�n fak�lte ID'sine g�re filtrele
                        .ToList();
                }
            }
            
            return View("tables-data",s�n�flar); // E�er s�n�flar bo�sa ana sayfaya y�nlendiriyoruz
        }

        [Route("Home/TablesGeneral/{s�n�fID}")]
        public IActionResult TablesGeneral(int s�n�fID)
        {
            // Fak�lte numaras�n� oturumdan al�yoruz (giri� yapm��sa)
            var fakulteNo = HttpContext.Session.GetInt32("FakulteID");

            // Kullan�c� tipi oturumda var m�, kontrol edelim
            var userType = HttpContext.Session.GetString("UserType");

            // S�n�flar� �ekiyoruz ve Admin veya Fak�lteye g�re filtreliyoruz
            List<S�n�f> s�n�flar = _context.S�n�flar
                .Include(s => s.Fakulte)
                .ToList(); // T�m s�n�flar� al�yoruz, sadece filtreyi uyguluyoruz

            // E�er kullan�c� Admin ise veya fak�lte numaras� e�le�iyorsa s�n�flar� filtreliyoruz
            if (userType == "Admin")
            {
                // Admin t�m s�n�flar� g�rebilir, ekstra bir filtrelemeye gerek yok
            }
            else if (fakulteNo.HasValue)
            {
                // E�er fak�lte numaras� varsa, sadece o fak�lteye ait s�n�flar� al�yoruz
                s�n�flar = s�n�flar.Where(s => s.FakulteID == fakulteNo.Value).ToList();
            }

            // Se�ilen s�n�f� filtreliyoruz
            var selectedClass = s�n�flar.FirstOrDefault(s => s.S�n�fID == s�n�fID);

            if (selectedClass == null)
            {
                TempData["ErrorMessage"] = "Se�ilen s�n�f bulunamad�.";
                return RedirectToAction("TablesData"); // Geri y�nlendirme
            }

            // Se�ilen s�n�f�n derslerini al�yoruz
            var dersler = _context.EklenenDersler
                .Where(o => o.S�n�fID == s�n�fID)
                .ToList();

            // S�n�fa ait oturumlar� filtreliyoruz
            var gunler = dersler.Select(o => o.Gun).Distinct().OrderBy(g => g).ToList();
            var saatler = dersler
                .Select(o => new SaatAraligi { BaslangicSaati = o.BaslangicSaati, BitisSaati = o.BitisSaati })
                .Distinct()
                .ToList();

            var bolumler = dersler.Select(d => d.BolumAdi).Distinct().OrderBy(b => b).ToList();

            // Verileri ViewBag �zerinden g�nderiyoruz
            ViewBag.Saatler = saatler;
            ViewBag.SelectedClassName = selectedClass.S�n�fAd; // Se�ilen s�n�f ad�
            ViewBag.Gunler = gunler;
            ViewBag.Oturumlar = dersler;
            ViewBag.Bolumler = bolumler;
            ViewBag.S�n�flar = s�n�flar; // Admin ya da kullan�c�n�n fak�ltesine g�re filtrelenmi� s�n�flar

            // Sayfay� d�nd�r�yoruz
            return View("tables-general");


        }
        //public IActionResult DersEkle(int s�n�fID)
        //{
        //    // Oturumdaki kullan�c� bilgilerini al
        //    var userType = HttpContext.Session.GetString("UserType");
        //    var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fak�lte numaras�

        //    if (userType == null)
        //    {
        //        TempData["ErrorMessage"] = "Giri� yapman�z gerekiyor.";
        //        return RedirectToAction("Login", "Account"); // Kullan�c� giri� yapmam��sa y�nlendirme yap
        //    }

        //    // Kullan�c�ya g�re s�n�flar� filtrele
        //    List<S�n�f> s�n�flar = _context.S�n�flar
        //        .Include(s => s.Fakulte)
        //        .Where(s => userType == "Admin" || s.FakulteID == fakulteNo)
        //        .ToList();

        //    return View("dersekleme", s�n�flar);
        //}


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
