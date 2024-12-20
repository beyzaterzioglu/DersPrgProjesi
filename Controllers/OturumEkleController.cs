using DersPrgProjesi.Data;
using DersPrgProjesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DersPrgProjesi.Controllers
{
    public class OturumEkleController : Controller
    {
        private readonly ApplicationDbContext _context; // DbContext nesnesi
        private readonly ILogger<HomeController> _logger;

        public OturumEkleController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Oturum Ekleme Formu
        public IActionResult OturumEkle()
        {
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
            return View("OturumEkle", sınıflar);
        }

        // POST: Oturum Ekleme
        [HttpPost]
        public IActionResult OturumEkle(TimeSpan BaslangicSaati, TimeSpan BitisSaati, DayOfWeek Gun, string SınıfAd)
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

        

            if (ModelState.IsValid)
            {
                // Sınıf ID'yi bulmak için SınıfAd'ı kullan
                var sınıfId = _context.Sınıflar
                    .Where(s => s.SınıfAd == SınıfAd)
                    .Select(s => s.SınıfID)
                    .FirstOrDefault();

                if (sınıfId == 0)
                {
                    ModelState.AddModelError("SınıfAd", "Seçilen sınıf bulunamadı.");
                    return View();
                }

                // Yeni Oturum nesnesi oluştur
                var yeniOturum = new Oturum
                {
                    BaslangicSaati = BaslangicSaati,
                    BitisSaati = BitisSaati,
                    Gun = Gun,
                    SınıfID = sınıfId
                };

                // Veritabanına kaydet
                _context.Oturumlar.Add(yeniOturum);
                _context.SaveChanges();

                // Başarıyla yönlendir
                return RedirectToAction("Index", "Home");
            }

            // Eğer ModelState hatalıysa aynı sayfaya dön
            return View();

        }

    }
}
