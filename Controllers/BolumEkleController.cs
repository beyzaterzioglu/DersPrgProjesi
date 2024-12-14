using DersPrgProjesi.Data;
using DersPrgProjesi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DersPrgProjesi.Controllers
{
    public class BolumEkleController : Controller
    {
        private readonly ApplicationDbContext _context; // DbContext nesnesi
        private readonly ILogger<FakulteKayıtController> _logger;

        public BolumEkleController(ApplicationDbContext context, ILogger<FakulteKayıtController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult BolumEkle()
        {
            //var userType = HttpContext.Session.GetString("UserType");
            //if (userType != "Admin" && userType != "Fakulte")
            //{
            //    TempData["ErrorMessage"] = "Bu sayfaya erişim yetkiniz yok.";
            //    return RedirectToAction("Index", "Home");
            //}
            //var sınıflar = _context.Sınıflar.ToList();

            //return View("bolumekle", sınıflar);

            // Kullanıcı tipini ve FakülteNo'yu oturumdan al
            var userType = HttpContext.Session.GetString("UserType");
            var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fakülte numarası

            // Sınıfları filtrele
            List<Sınıf> sınıflar;

            if (userType == "Admin")
            {
                // Admin tüm sınıfları görür
                sınıflar = _context.Sınıflar.ToList();
            }
            else if (userType == "Fakulte" || userType == "Bolum")
            {
                // Fakülte ve Bölüm kullanıcıları sadece kendi fakültelerindeki sınıfları görür
                sınıflar = _context.Sınıflar
                    .Where(s => s.FakulteID == fakulteNo)
                    .ToList();
            }
            else
            {
                // Eğer kullanıcı tipi tanımlı değilse, hata mesajı ile geri döndür
                TempData["ErrorMessage"] = "Bu sayfaya erişim yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }

            // Sınıfları View'e gönder
            return View("bolumekle", sınıflar);
        }
        public IActionResult KullanıcıKayıt()
        {
            return View("bolumekle");
        }

        [HttpPost]
        public IActionResult BolumEkle(string BolumAd, string BolumMail, string BolumSifre, string FakulteAd)
        {
            if (ModelState.IsValid)
            {
                // FakulteID'yi almak için FakulteAd'ı kullanıyoruz.
                var fakulteId = _context.Fakulteler
                              .Where(f => f.FakulteAd == FakulteAd)
                              .Select(f => f.FakulteID)
                              .FirstOrDefault();

                // Yeni bir Bolum nesnesi oluştur
                var yeniBolum = new Bolum
                {
                    BolumAd = BolumAd,
                    BolumMail = BolumMail,
                    BolumSifre = BolumSifre,
                    FakulteID = fakulteId // Bulduğumuz FakulteID'yi set ediyoruz
                };

                // Veritabanına kaydet
                _context.Bolumler.Add(yeniBolum);
                _context.SaveChanges();

                // Başarı mesajı ile anasayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }

            // Eğer bir hata varsa, aynı sayfaya geri dön
            return View();
        }

        //    [HttpGet]
        //    [Route("api/FakulteKayit/GetFakulteId")]
        //    public IActionResult GetFakulteId()
        //    {
        //        var fakulteid = _context.Fakulteler
        //                      .Where(f => f.FakulteAd == )
        //                      .Select(f => new { f.FakulteID })
        //                      .ToList();
        //        return Ok(fakulteid);
        //    }
        //}
    }
}
