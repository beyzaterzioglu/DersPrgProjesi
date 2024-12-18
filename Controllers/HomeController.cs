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
                HttpContext.Session.SetInt32("FakulteID", faculty.FakulteID); // Fakülte ID kaydediliyor
                return RedirectToAction("Index");
            }

            // Bölüm kontrolü
            var department = _context.Bolumler
                .FirstOrDefault(d => d.BolumMail == Username && d.BolumSifre == Password);
            if (department != null)
            {
                HttpContext.Session.SetString("UserType", "Bolum");
                HttpContext.Session.SetInt32("BolumID", department.BolumID); // Bölüm ID kaydediliyor
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
            //// Oturum kontrolü yap
            //if (HttpContext.Session.GetString("UserType") == null)
            //{
            //    // Eðer oturum açýlmamýþsa, Login sayfasýna yönlendir
            //    return RedirectToAction("Login");
            //}

            //var sýnýflar = _context.Sýnýflar.ToList(); // Sýnýf verisini alýyoruz 

            //return View("index", sýnýflar); // index.cshtml

            // Oturum kontrolü yap
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(userType))
            {
                // Eðer oturum açýlmamýþsa, Login sayfasýna yönlendir
                return RedirectToAction("Login");
            }

            // Kullanýcý türüne göre sýnýflarý filtrele
            var sýnýflar = new List<Sýnýf>();

            if (userType == "Admin")
            {
                // Admin tüm sýnýflarý görebilir
                sýnýflar = _context.Sýnýflar.ToList();
            }
            else if (userType == "Fakulte")
            {
                // Fakülte sadece kendi fakültesine ait sýnýflarý görebilir
                var fakulteId = HttpContext.Session.GetInt32("FakulteID");
                sýnýflar = _context.Sýnýflar
                    .Where(s => s.FakulteID == fakulteId) // FakülteID ile filtrele
                    .ToList();
            }
            else if (userType == "Bolum")
            {
                // Bölüm sadece kendi fakültesine ait sýnýflarý görebilir
                var bolumId = HttpContext.Session.GetInt32("BolumID");
                var bolum = _context.Bolumler.FirstOrDefault(b => b.BolumID == bolumId);

                if (bolum != null)
                {
                    sýnýflar = _context.Sýnýflar
                        .Where(s => s.FakulteID == bolum.FakulteID) // Bölümün fakülte ID'sine göre filtrele
                        .ToList();
                }
            }

            return View("index", sýnýflar); // index.cshtml'e sýnýf listesini gönder
        }
        public IActionResult BolumEkle() // Anasayfa
        {
            return View("bolumekle");
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

        //    var sýnýflar = _context.Sýnýflar
        //.Include(s => s.Fakulte)  // Fakülte bilgilerini de dahil ediyoruz
        //.ToList();

        //    if (sýnýflar != null && sýnýflar.Any())
        //    {
        //        return View("tables-data", sýnýflar); // Fakülte bilgisiyle birlikte model gönderiyoruz.
        //    }
        //    return View("index"); // Eðer veriler boþsa index'e yönlendiririz



            // Kullanýcý tipini ve FakülteNo'yu oturumdan al
            var userType = HttpContext.Session.GetString("UserType");
            var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fakülte numarasý

            List<Sýnýf> sýnýflar;

            if (userType == "Admin")
            {
                // Admin tüm sýnýflarý görebilir
                sýnýflar = _context.Sýnýflar
                    .Include(s => s.Fakulte) // Fakülte bilgilerini de dahil ediyoruz
                    .ToList();
            }
            else if (userType == "Fakulte" || userType == "Bolum")
            {
                // Fakülte ve Bölüm kullanýcýlarý yalnýzca kendi fakültelerine ait sýnýflarý görür
                sýnýflar = _context.Sýnýflar
                    .Include(s => s.Fakulte) // Fakülte bilgilerini dahil ediyoruz
                    .Where(s => s.FakulteID == fakulteNo)
                    .ToList();
            }
            else
            {
                // Geçersiz bir kullanýcý tipi ile karþýlaþýlýrsa ana sayfaya yönlendir
                TempData["ErrorMessage"] = "Bu sayfaya eriþim yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }

            if (sýnýflar != null && sýnýflar.Any())
            {
                return View("tables-data", sýnýflar); // Modeli doðru þekilde View'a gönderiyoruz
            }

            return View("index"); // Eðer sýnýflar boþsa ana sayfaya yönlendiriyoruz
        }

        public IActionResult TablesGeneral(int sýnýfID)
        {



            //// Kullanýcý tipini ve FakülteNo'yu oturumdan al
            //var userType = HttpContext.Session.GetString("UserType");
            //var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fakülte numarasý

            //List<Sýnýf> sýnýflar;

            //if (userType == "Admin")
            //{
            //    // Admin tüm sýnýflarý görebilir
            //    sýnýflar = _context.Sýnýflar
            //        .Include(s => s.Fakulte)
            //        .ToList();
            //}
            //else if (userType == "Fakulte" || userType == "Bolum")
            //{
            //    // Fakülte ve Bölüm kullanýcýlarý yalnýzca kendi fakültelerine ait sýnýflarý görür
            //    sýnýflar = _context.Sýnýflar
            //        .Include(s => s.Fakulte)
            //        .Where(s => s.FakulteID == fakulteNo)
            //        .ToList();
            //}
            //else
            //{
            //    // Geçersiz bir kullanýcý tipi ile karþýlaþýlýrsa ana sayfaya yönlendir
            //    TempData["ErrorMessage"] = "Bu sayfaya eriþim yetkiniz yok.";
            //    return RedirectToAction("Index", "Home");
            //}

            //// Eðer spesifik bir sýnýfID kontrolü gerekiyorsa:
            //var selectedClass = sýnýflar.FirstOrDefault(s => s.SýnýfID == sýnýfID);

            //if (selectedClass != null)
            //{
            //    return View("tables-general", selectedClass); // Seçilen sýnýfý View'a gönderiyoruz
            //}

            //TempData["ErrorMessage"] = "Belirtilen sýnýf bulunamadý.";
            //return RedirectToAction("TablesData");



            /// yeni


            // Oturumdaki kullanýcý bilgilerini al
            var userType = HttpContext.Session.GetString("UserType");
            var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fakülte numarasý

            if (userType == null)
            {
                TempData["ErrorMessage"] = "Giriþ yapmanýz gerekiyor.";
                return RedirectToAction("Login", "Account"); // Kullanýcý giriþ yapmamýþsa yönlendirme yap
            }

            // Kullanýcýya göre sýnýflarý filtrele
            List<Sýnýf> sýnýflar = _context.Sýnýflar
                .Include(s => s.Fakulte)
                .Where(s => userType == "Admin" || s.FakulteID == fakulteNo)
                .ToList();

            //// Belirtilen sýnýfý bul
            //var selectedClass = sýnýflar.FirstOrDefault(s => s.SýnýfID == sýnýfID);
            //if (selectedClass == null)
            //{
            //    TempData["ErrorMessage"] = "Seçilen sýnýf bulunamadý veya eriþim izniniz yok.";
            //    return RedirectToAction("TablesData"); // Geri yönlendirme
            //}

            //return View("tables-general", selectedClass); // Seçilen sýnýfý View'e gönder
            return View("tables-general", sýnýflar);



        }
        public IActionResult DersEkle(int sýnýfID)
        {
            // Oturumdaki kullanýcý bilgilerini al
            var userType = HttpContext.Session.GetString("UserType");
            var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fakülte numarasý

            if (userType == null)
            {
                TempData["ErrorMessage"] = "Giriþ yapmanýz gerekiyor.";
                return RedirectToAction("Login", "Account"); // Kullanýcý giriþ yapmamýþsa yönlendirme yap
            }

            // Kullanýcýya göre sýnýflarý filtrele
            List<Sýnýf> sýnýflar = _context.Sýnýflar
                .Include(s => s.Fakulte)
                .Where(s => userType == "Admin" || s.FakulteID == fakulteNo)
                .ToList();

            return View("dersekleme", sýnýflar);
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
