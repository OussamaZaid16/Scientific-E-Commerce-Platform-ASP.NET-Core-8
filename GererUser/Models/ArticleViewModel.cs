using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ScienceStore.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de l'article est obligatoire.")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
        [Display(Name = "Titre de l'article")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Range(0.01, 10000, ErrorMessage = "Le prix doit être supérieur à 0.")]
        [Display(Name = "Prix (€)")]
        public decimal Prix { get; set; }

        [Display(Name = "Catégorie / Domaine")]
        [Required(ErrorMessage = "Veuillez sélectionner un domaine.")]
        public string? Domaine { get; set; }

        [Display(Name = "Description courte")]
        public string? DescriptionCourte { get; set; }

        // --- AJOUT DE LA DESCRIPTION LONGUE ICI ---
        [Display(Name = "Description détaillée")]
        public string? Description { get; set; }

        [Display(Name = "Note moyenne")]
        [Range(0, 5, ErrorMessage = "La note doit être entre 0 et 5.")]
        public double NoteMoyenne { get; set; }

        [Display(Name = "Changer l'image")]
        public IFormFile? FichierImage { get; set; }

        [Display(Name = "Image actuelle")]
        public string? NomFichierImageExistant { get; set; }
    }
}