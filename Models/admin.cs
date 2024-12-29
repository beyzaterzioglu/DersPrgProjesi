using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DersPrgProjesi.Models
{
    public class admin
    {
        
        [Key] // Admin tablosunun anahtar (primary key) sütunu
        public int id { get; set; }

        [Required] // Bu alanın boş geçilemeyeceğini belirtir
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
