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
        public int? FakulteID { get; set; }  
        public virtual Fakulte Fakulte { get; set; }  // Fakülteyi referans alır
    }
}
