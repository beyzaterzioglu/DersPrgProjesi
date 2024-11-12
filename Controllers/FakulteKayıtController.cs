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
            return View("kullanıcıkayıt");
        }
        [HttpPost]
        public IActionResult FakulteKayıt(Fakulte fakulte)
        {
            // Yeni kullanıcı nesnesi oluştur
            if (ModelState.IsValid)
            {
                // Veritabanına fakülteyi ekle
                _context.Fakulteler.Add(fakulte);
                _context.SaveChanges();
                return View("index");  // Başarıyla kaydedildikten sonra anasayfaya yönlendir
            }
            // Veritabanına kaydet

            else
            {
                return View("kullanıcıkayıt");
            }
            // Kayıt başarılıysa giriş sayfasına veya ana sayfaya yönlendir
           
        }
    }
}

