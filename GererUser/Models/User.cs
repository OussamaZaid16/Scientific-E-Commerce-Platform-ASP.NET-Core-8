using System.ComponentModel.DataAnnotations;

namespace GestionUsersMVC.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string MotDePasse { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
