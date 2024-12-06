using System.ComponentModel.DataAnnotations;

namespace DersPrgProjesi.Models
{
    public class Bolum
    {
        [Key]
        public int BolumID { get; set; }
        [Required]
        public string BolumAd { get; set; }
        [Required]
        public string BolumMail { get; set; }
        [Required]
        public string BolumSifre { get; set; }
        // Foreign key for Fakulte
        public int? FakulteID { get; set; }  // Nullable, çünkü bazı durumlarda FakülteID olmayabilir

        // Navigation property for Fakulte
        public virtual Fakulte Fakulte { get; set; }  // Fakülteyi referans alır
    }
}
