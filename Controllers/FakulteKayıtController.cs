using DersPrgProjesi.Data;
using DersPrgProjesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DersPrgProjesi.Controllers
{
   

    public class FakulteKayıtController : Controller
    {
        private readonly ApplicationDbContext _context; // DbContext nesnesi
        private readonly ILogger<FakulteKayıtController> _logger;

        public FakulteKayıtController(ApplicationDbContext context, ILogger<FakulteKayıtController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult FakulteKayıt()
        {
            //var userType = HttpContext.Session.GetString("UserType");
            //if (userType != "Admin")
            //{
            //    TempData["ErrorMessage"] = "Bu sayfaya erişim yetkiniz yok.";
            //    return RedirectToAction("Index", "Home");
            //}
            //var sınıflar = _context.Sınıflar.ToList();
            //return View("kullanıcıkayıt",sınıflar);

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
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "Admin")
            {
                TempData["ErrorMessage"] = "Bu sayfaya erişim yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }

            return View("kullanıcıkayıt");
        }
        //public IActionResult Index() // Anasayfa
        //{
        //    return View("index"); // index.cshtml
        //}

        [HttpPost]
        public IActionResult FakulteKayıt(Fakulte fakulte, string inputPasswordRepeat)
        {
            if (ModelState.IsValid)
            {
                if (fakulte.FakulteSifre != inputPasswordRepeat)
                {
                    ModelState.AddModelError("", "Şifreler uyuşmuyor.");
                    return View("kullanıcıkayıt");
                }

                _context.Fakulteler.Add(fakulte);
                _context.SaveChanges();
                return RedirectToAction("index", "Home"); // Ana sayfaya yönlendirme // Kaydedildikten sonra Index sayfasına yönlendirme
            }
            //if (!ModelState.IsValid)
            //{
            //    // Hata mesajlarını görmek için:
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    _logger.LogError("ModelState hataları: {Errors}", errors);
            //}
            return RedirectToAction("KullanıcıKayıt","FakulteKayıt");
        }
        [HttpGet]
        
        //public IActionResult GetFakulteIds()
        //{
        //    // FakulteAd değeri "Mühendislik ve Mimarlık Fakültesi" olan kayıtları sorgulayın
        //    var fakulteIds = _context.Fakulteler
        //                             .Where(f => f.FakulteAd == "Mühendislik ve Mimarlık Fakültesi")
        //                             .Select(f => f.FakulteID) // sadece Id alanını seçiyoruz
        //                             .ToList();

        //    // ID'leri döndürün
        //    return Ok(fakulteIds);
        //}

        [HttpGet]
        [Route("api/FakulteKayit/GetFakulteler")]
        public IActionResult GetFakulteler()
        {
            var fakulteler = _context.Fakulteler
                          .Select(f => new { f.FakulteAd })
                          .ToList();
            return Ok(fakulteler);
        }


    }
}

