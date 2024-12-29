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
        public IActionResult OturumEkle(string sınıfAdı)
        {
            Console.WriteLine(sınıfAdı);
            var userType = HttpContext.Session.GetString("UserType");
            //var fakulteNo = HttpContext.Session.GetInt32("FakulteID"); // Fakülte numarası

            //List<Sınıf> sınıflar;
            var sınıflar = new List<Sınıf>();

            if (userType == "Admin")
            {

                sınıflar = _context.Sınıflar.ToList();
            }
            else if (userType == "Fakulte")
            {
                var fakulteId = HttpContext.Session.GetInt32("FakulteID");
                sınıflar = _context.Sınıflar
                    .Where(s => s.FakulteID == fakulteId) // FakülteID ile filtrele
                    .ToList();
            }
            else if (userType == "Bolum")
            {
                // Bölüm sadece kendi fakültesine ait sınıfları görebilir
                var bolumId = HttpContext.Session.GetInt32("BolumID");
                var bolum = _context.Bolumler.FirstOrDefault(b => b.BolumID == bolumId);

                if (bolum != null)
                {
                    sınıflar = _context.Sınıflar
                        .Where(s => s.FakulteID == bolum.FakulteID) // Bölümün fakülte ID'sine göre filtrele
                        .ToList();
                }
            }

            // Sınıfları View'e gönder
            return View("OturumEkle", sınıflar);
        }

        // POST: Oturum Ekleme
        [HttpPost]
        public IActionResult OturumEkle(TimeSpan BaslangicSaati, TimeSpan BitisSaati, DayOfWeek Gun, string sınıfAdı, string DersAdi, string BolumAdi)
        {

            Console.WriteLine(sınıfAdı);
            var userType = HttpContext.Session.GetString("UserType");
           

            List<Sınıf> sınıflar;


            if (userType == "Admin")
            {
                sınıflar = _context.Sınıflar.ToList();
                
            }
            else if (userType == "Fakulte")
            {
                var fakulteNo = HttpContext.Session.GetInt32("FakulteID");
                sınıflar = _context.Sınıflar
                    .Where(s => s.FakulteID == fakulteNo)
                    .ToList();
              
            }
            else if (userType == "Bolum")
            {
                // Bölüm sadece kendi fakültesine ait sınıfları görebilir
                var bolumId = HttpContext.Session.GetInt32("BolumID");
                var bolum = _context.Bolumler.FirstOrDefault(b => b.BolumID == bolumId);


                    sınıflar = _context.Sınıflar
                        .Where(s => s.FakulteID == bolum.FakulteID) // Bölümün fakülte ID'sine göre filtrele
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
                    .Where(s => s.SınıfAd == sınıfAdı)
                    .Select(s => s.SınıfID)
                    .FirstOrDefault();

                if (sınıfId == 0)
                {
                    ModelState.AddModelError("SınıfAd", "Seçilen sınıf bulunamadı.");
                    return View();
                }

                // Yeni Oturum nesnesi oluştur
                var yeniOturum = new EklenenDers
                {
                    BaslangicSaati = BaslangicSaati,
                    BitisSaati = BitisSaati,
                    Gun = Gun,
                    SınıfID = sınıfId,
                    DersAdi = DersAdi,
                    BolumAdi = BolumAdi
                };

                // Veritabanına kaydet
                _context.EklenenDersler.Add(yeniOturum);
                _context.SaveChanges();

                // Başarıyla yönlendir
                return RedirectToAction("Index", "Home");
            }
            //if (!ModelState.IsValid)
            //{
            //    // Hataları al ve bir string değişkene ata
            //    var errors = ModelState.Values
            //        .SelectMany(v => v.Errors)
            //        .Select(e => e.ErrorMessage + (e.Exception != null ? $" - {e.Exception.Message}" : ""))
            //        .ToList();

            //    // Hataları loglama veya ekrana yazdırma
            //    foreach (var error in errors)
            //    {
            //        Console.WriteLine(error); // Bu satırı loglama mekanizmanıza uyarlayabilirsiniz.
            //    }

            //    // TempData ile hata mesajını kullanıcıya göstermek isterseniz
            //    TempData["ErrorDetails"] = string.Join("; ", errors);

            //    return View(sınıflar);
            //}

            // Eğer ModelState hatalıysa aynı sayfaya dön
            return View(sınıflar);



        }

    }
}
