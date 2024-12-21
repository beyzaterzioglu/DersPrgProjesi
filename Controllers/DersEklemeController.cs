using DersPrgProjesi.Data;
using DersPrgProjesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DersPrgProjesi.Controllers
{
    public class DersEklemeController : Controller
    {
        private readonly ApplicationDbContext _context; // DbContext nesnesi
        private readonly ILogger<FakulteKayıtController> _logger;

        public DersEklemeController(ApplicationDbContext context, ILogger<FakulteKayıtController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //public IActionResult DersEkleme()
        //{
        //    var userType = HttpContext.Session.GetString("UserType");
        //    var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fakülte numarası

        //    // Sınıfları filtrele
        //    List<Sınıf> sınıflar;

        //    if (userType == "Admin")
        //    {
        //        // Admin tüm sınıfları görür
        //        sınıflar = _context.Sınıflar.ToList();
        //    }
        //    else if (userType == "Fakulte" || userType == "Bolum")
        //    {
        //        // Fakülte ve Bölüm kullanıcıları sadece kendi fakültelerindeki sınıfları görür
        //        sınıflar = _context.Sınıflar
        //            .Where(s => s.FakulteID == fakulteNo)
        //            .ToList();
        //    }
        //    else
        //    {
        //        // Eğer kullanıcı tipi tanımlı değilse, hata mesajı ile geri döndür
        //        TempData["ErrorMessage"] = "Bu sayfaya erişim yetkiniz yok.";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // Sınıfları View'e gönder
        //    return View("tablesgeneral", sınıflar);
        //}


        public IActionResult DersEkleme(TimeSpan BaslangicSaati, TimeSpan BitisSaati, DayOfWeek Gun, string SınıfAd)
        {

            var userType = HttpContext.Session.GetString("UserType");
            var fakulteNo = HttpContext.Session.GetInt32("FakulteID");

            List<Sınıf> sınıflar;

            if (userType == "Admin")
            {
                sınıflar = _context.Sınıflar.ToList();
            }
            else if (userType == "Fakulte" || userType == "Bolum")
            {
                sınıflar = _context.Sınıflar
                    .Where(s => s.FakulteID == fakulteNo)
                    .ToList();
            }
            else
            {
                TempData["ErrorMessage"] = "Bu sayfaya erişim yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }



            //if (ModelState.IsValid)
            //{
            //    // Sınıf ID'yi bulmak için SınıfAd'ı kullan
            //    var sınıfId = _context.Sınıflar
            //        .Where(s => s.SınıfAd == SınıfAd)
            //        .Select(s => s.SınıfID)
            //        .FirstOrDefault();

            //    if (sınıfId == 0)
            //    {
            //        ModelState.AddModelError("SınıfAd", "Seçilen sınıf bulunamadı.");
            //        return View();
            //    }

            //    // Yeni Oturum nesnesi oluştur
            //    var yeniDers = new Ders
            //    {
            //        BaslangicSaati = BaslangicSaati,
            //        BitisSaati = BitisSaati,
            //        Gun = Gun,
            //        SınıfID = sınıfId
            //    };

            //    // Veritabanına kaydet
            //    _context.Ders.Add(yeniDers);
            //    _context.SaveChanges();

            //    // Başarıyla yönlendir
            //    return View("tablesgeneral", sınıflar);
            //}

            // Eğer ModelState hatalıysa aynı sayfaya dön
            return View("~/Views/OturumEkle/OturumEkle.cshtml", sınıflar);

        }
        [HttpGet]
        [Route("api/DersEkleme/GetBolumler")]
        public IActionResult GetBolumler()
        {
            var bolumler = _context.Bolumler
                          .Select(f => new { f.BolumAd })
                          .ToList();
            return Ok(bolumler);
        }
    }
}
