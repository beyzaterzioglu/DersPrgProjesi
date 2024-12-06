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
            var sınıflar = _context.Sınıflar.ToList();
            return View("sınıfekleme",sınıflar); // Sınıf ekleme formunu görüntüle
        }

        // POST: SınıfEkle
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult SınıfEkle(string SınıfAd, int Kapasite, int SınavKapasite, string FakulteAd)
        {
            if (ModelState.IsValid)
            {
                // FakulteID'yi almak için FakulteAd'ı kullanıyoruz.
                var fakulteId = _context.Fakulteler
                              .Where(f => f.FakulteAd == FakulteAd)
                              .Select(f => f.FakulteID)
                              .FirstOrDefault();

                // Yeni bir Bolum nesnesi oluştur
                var yeniSınıf = new Sınıf
                {
                    SınıfAd = SınıfAd,
                    Kapasite = Kapasite,
                    SınavKapasite = SınavKapasite,
                    FakulteID = fakulteId
                };


                // Veritabanına kaydet
                _context.Sınıflar.Add(yeniSınıf);
                _context.SaveChanges();

                // Başarı mesajı ile anasayfaya yönlendir
                return View("sınıfekleme");
            }


            
            return RedirectToAction("Index", "Home");

            
        }

    }
}
