using System.ComponentModel.DataAnnotations;

namespace DersPrgProjesi.Models
{
    public class EklenenDers
    {

        [Key]
        public int OturumId { get; set; } // Primary Key

        [Required]
        public TimeSpan BaslangicSaati { get; set; } // Oturumun başlangıç saati

        [Required]
        public TimeSpan BitisSaati { get; set; } // Oturumun bitiş saati

        [Required]
        public DayOfWeek Gun { get; set; } // Haftanın günü (Pazartesi, Salı vb.)

        public int? SınıfID { get; set; } // Foreign Key (Sınıf tablosuna bağlanacak)
        public virtual Sınıf Sınıf { get; set; }
        [Required]
        public string DersAdi { get; set; } // Ders adı

        [Required]
        public string BolumAdi { get; set; } // Bölüm adı
    }
}
