using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DersPrgProjesi.Models
{
    public class Sınıf
    {
        [Key]
        public int SınıfID { get; set; }

        [Required]
        [StringLength(100)]
        public string SınıfAd { get; set; }

        [Required]
        public int Kapasite { get; set; }

        public int SınavKapasite { get; set; }

        public int? FakulteID { get; set; }  // Nullable, çünkü bazı durumlarda FakülteID olmayabilir

        // Navigation property for Fakulte
        public virtual Fakulte Fakulte { get; set; }  // Fakülteyi referans alır
    }
}
