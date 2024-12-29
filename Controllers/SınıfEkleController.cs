using DersPrgProjesi.Data;
using DersPrgProjesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DersPrgProjesi.Controllers
{
    public class SınıfEkleController : Controller
    {
        private readonly ApplicationDbContext _context; // DbContext nesnesi
        private readonly ILogger<SınıfEkleController> _logger;

        public SınıfEkleController(ApplicationDbContext context, ILogger<SınıfEkleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: SınıfEkle
        public IActionResult SınıfEkle()
        {
            //var sınıflar = _context.Sınıflar.ToList();
            //return View("sınıfekleme",sınıflar); // Sınıf ekleme formunu görüntüle

            

            // Sınıfları filtrele
            var userType = HttpContext.Session.GetString("UserType");
            
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
            return View("~/Views/SınıfEkle/sınıfekleme.cshtml", sınıflar);
        }

        // POST: SınıfEkle
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public IActionResult SınıfEkle(string SınıfAd, int Kapasite, int SınavKapasite, string FakulteAd)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // FakulteID'yi almak için FakulteAd'ı kullanıyoruz.
        //        var fakulteId = _context.Fakulteler
        //                      .Where(f => f.FakulteAd == FakulteAd)
        //                      .Select(f => f.FakulteID)
        //                      .FirstOrDefault();

        //        // Yeni bir Bolum nesnesi oluştur
        //        var yeniSınıf = new Sınıf
        //        {
        //            SınıfAd = SınıfAd,
        //            Kapasite = Kapasite,
        //            SınavKapasite = SınavKapasite,
        //            FakulteID = fakulteId
        //        };


        //        // Veritabanına kaydet
        //        _context.Sınıflar.Add(yeniSınıf);
        //        _context.SaveChanges();

        //        // Başarı mesajı ile anasayfaya yönlendir
        //        return View("sınıfekleme");
        //    }



        //    return RedirectToAction("Index", "Home");


        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SınıfEkle(string SınıfAd, int Kapasite, int SınavKapasite, string FakulteAd)
        {
            if (ModelState.IsValid)
            {
                var fakulteId = _context.Fakulteler
                    .Where(f => f.FakulteAd == FakulteAd)
                    .Select(f => f.FakulteID)
                    .FirstOrDefault();

                var yeniSınıf = new Sınıf
                {
                    SınıfAd = SınıfAd,
                    Kapasite = Kapasite,
                    SınavKapasite = SınavKapasite,
                    FakulteID = fakulteId
                };

                _context.Sınıflar.Add(yeniSınıf);
                _context.SaveChanges();

                // Kayıttan sonra güncel listeyi getir
                var sınıflar = _context.Sınıflar.ToList();
                return View("sınıfekleme", sınıflar);
            }

            // Eğer validasyon başarısızsa, mevcut listeyi tekrar gönder
            var mevcutSınıflar = _context.Sınıflar.ToList();
            return View("sınıfekleme", mevcutSınıflar);
        }

        [HttpGet]
        [Route("api/SinifEkle/GetSiniflar")]
        public IActionResult GetFakulteler()
        {
            var sınıflar = _context.Sınıflar
                          .Select(f => new { f.SınıfAd })
                          .ToList();
            return Ok(sınıflar);
        }
    }
}
