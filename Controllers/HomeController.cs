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
            //var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fakülte numarasý

            //List<Sýnýf> sýnýflar;
            var sýnýflar = new List<Sýnýf>();

            if (userType == "Admin")
            {
           
              sýnýflar = _context.Sýnýflar.ToList();
            }
            else if (userType == "Fakulte")
            {
                var fakulteId = HttpContext.Session.GetInt32("FakulteID");
                sýnýflar = _context.Sýnýflar
                    .Where(s => s.FakulteID == fakulteId) // FakülteID ile filtrele
                    .ToList();
            }
            else if ( userType == "Bolum")
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
            
            return View("tables-data",sýnýflar); // Eðer sýnýflar boþsa ana sayfaya yönlendiriyoruz
        }

        [Route("Home/TablesGeneral/{sýnýfID}")]
        public IActionResult TablesGeneral(int sýnýfID)
        {
            // Fakülte numarasýný oturumdan alýyoruz (giriþ yapmýþsa)
            var fakulteNo = HttpContext.Session.GetInt32("FakulteID");

            // Kullanýcý tipi oturumda var mý, kontrol edelim
            var userType = HttpContext.Session.GetString("UserType");

            // Sýnýflarý çekiyoruz ve Admin veya Fakülteye göre filtreliyoruz
            List<Sýnýf> sýnýflar = _context.Sýnýflar
                .Include(s => s.Fakulte)
                .ToList(); // Tüm sýnýflarý alýyoruz, sadece filtreyi uyguluyoruz

            // Eðer kullanýcý Admin ise veya fakülte numarasý eþleþiyorsa sýnýflarý filtreliyoruz
            if (userType == "Admin")
            {
                // Admin tüm sýnýflarý görebilir, ekstra bir filtrelemeye gerek yok
            }
            else if (fakulteNo.HasValue)
            {
                // Eðer fakülte numarasý varsa, sadece o fakülteye ait sýnýflarý alýyoruz
                sýnýflar = sýnýflar.Where(s => s.FakulteID == fakulteNo.Value).ToList();
            }

            // Seçilen sýnýfý filtreliyoruz
            var selectedClass = sýnýflar.FirstOrDefault(s => s.SýnýfID == sýnýfID);

            if (selectedClass == null)
            {
                TempData["ErrorMessage"] = "Seçilen sýnýf bulunamadý.";
                return RedirectToAction("TablesData"); // Geri yönlendirme
            }

            // Seçilen sýnýfýn derslerini alýyoruz
            var dersler = _context.EklenenDersler
                .Where(o => o.SýnýfID == sýnýfID)
                .ToList();

            // Sýnýfa ait oturumlarý filtreliyoruz
            var gunler = dersler.Select(o => o.Gun).Distinct().OrderBy(g => g).ToList();
            var saatler = dersler
                .Select(o => new SaatAraligi { BaslangicSaati = o.BaslangicSaati, BitisSaati = o.BitisSaati })
                .Distinct()
                .ToList();

            var bolumler = dersler.Select(d => d.BolumAdi).Distinct().OrderBy(b => b).ToList();

            // Verileri ViewBag üzerinden gönderiyoruz
            ViewBag.Saatler = saatler;
            ViewBag.SelectedClassName = selectedClass.SýnýfAd; // Seçilen sýnýf adý
            ViewBag.Gunler = gunler;
            ViewBag.Oturumlar = dersler;
            ViewBag.Bolumler = bolumler;
            ViewBag.Sýnýflar = sýnýflar; // Admin ya da kullanýcýnýn fakültesine göre filtrelenmiþ sýnýflar

            // Sayfayý döndürüyoruz
            return View("tables-general");


        }
        //public IActionResult DersEkle(int sýnýfID)
        //{
        //    // Oturumdaki kullanýcý bilgilerini al
        //    var userType = HttpContext.Session.GetString("UserType");
        //    var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fakülte numarasý

        //    if (userType == null)
        //    {
        //        TempData["ErrorMessage"] = "Giriþ yapmanýz gerekiyor.";
        //        return RedirectToAction("Login", "Account"); // Kullanýcý giriþ yapmamýþsa yönlendirme yap
        //    }

        //    // Kullanýcýya göre sýnýflarý filtrele
        //    List<Sýnýf> sýnýflar = _context.Sýnýflar
        //        .Include(s => s.Fakulte)
        //        .Where(s => userType == "Admin" || s.FakulteID == fakulteNo)
        //        .ToList();

        //    return View("dersekleme", sýnýflar);
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
