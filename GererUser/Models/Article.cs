using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GestionUsersMVC.Models
{

        public class Article
        {
            public int Id { get; set; }

            // On ajoute le '?' ou on initialise avec string.Empty pour éviter les erreurs de validation
            // si le système pense que c'est null au moment de la création.
            [Required(ErrorMessage = "Le titre est obligatoire")]
            public string Titre { get; set; } = string.Empty;

            [Column(TypeName = "decimal(18,2)")]
            public decimal Prix { get; set; }

            public string? DescriptionCourte { get; set; }

            public double NoteMoyenne { get; set; }

            // Ce champ stockera le nom du fichier (ex: 450abc.jpg)
            // Le '?' est CRUCIAL ici pour que l'image soit optionnelle
            public string? NomFichierImage { get; set; }

            // Valeurs par défaut pour éviter les erreurs "NULL" dans la base de données
            public string Description { get; set; } = "Aucune description";
            public string Auteur { get; set; } = "Anonyme";
            public string Domaine { get; set; } = "Général";
            public int Annee { get; set; } = DateTime.Now.Year;

            public string? ImageUrl { get; set; }
        }
    }
