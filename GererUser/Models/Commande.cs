using System;
using System.ComponentModel.DataAnnotations;

namespace GestionUsersMVC.Models
{
    public class Commande
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        public string Prenom { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Telephone { get; set; }

        [Required]
        public string Adresse { get; set; }

        public DateTime DateCommande { get; set; } = DateTime.Now;
        public double Total { get; set; }
        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}