using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DersPrgProjesi.Models
{
    public class admin
    {
        
        [Key] // Admin tablosunun anahtar (primary key) sütunu
        public int id { get; set; }

        [Required] // Bu alanın boş geçilemeyeceğini belirtir
       // [StringLength(50)] // Maksimum uzunluk belirler (opsiyonel)
        public string Username { get; set; }

        [Required]
       // [StringLength(50)]
        public string Password { get; set; }
    }
}
