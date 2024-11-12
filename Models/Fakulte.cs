using System.ComponentModel.DataAnnotations;

namespace DersPrgProjesi.Models
{
    public class Fakulte
    {
        [Key]
        public int FakulteID { get; set; }
        [Required]
        public string FakulteAd { get; set; }
        [Required]
        public string FakulteMail { get; set; }
        [Required]  
        public string FakulteSifre { get; set; }
        
    }
}
