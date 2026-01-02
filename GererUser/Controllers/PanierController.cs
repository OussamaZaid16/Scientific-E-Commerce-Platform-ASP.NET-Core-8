using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GestionUsersMVC.Models;
using GestionUsersMVC.Data;
using GestionUsersMVC.Extensions;
using System.Collections.Generic;
using System.Linq; // Important pour utiliser .FirstOrDefault()

namespace GestionUsersMVC.Controllers
{
    public class PanierController : Controller
    {
        private readonly AppDbContext _context;

        public PanierController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Ajouter(int id)
        {
            var article = _context.Articles.Find(id);

            if (article != null)
            {
                var panier = HttpContext.Session.GetObjectFromJson<List<Article>>("Panier") ?? new List<Article>();
                panier.Add(article);
                HttpContext.Session.SetObjectAsJson("Panier", panier);
            }

            return RedirectToAction("Index");
        }

        // --- MÉTHODE AJOUTÉE ICI ---
        public IActionResult Supprimer(int id)
        {
            // 1. On récupère le panier actuel depuis la session
            var panier = HttpContext.Session.GetObjectFromJson<List<Article>>("Panier") ?? new List<Article>();

            // 2. On cherche l'article à supprimer dans la liste
            // On utilise FirstOrDefault pour trouver le premier article correspondant à cet ID
            var articleASupprimer = panier.FirstOrDefault(a => a.Id == id);

            if (articleASupprimer != null)
            {
                // 3. On le retire de la liste
                panier.Remove(articleASupprimer);

                // 4. On remet le panier à jour dans la session
                HttpContext.Session.SetObjectAsJson("Panier", panier);
            }

            // 5. On revient sur la page du panier
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var panier = HttpContext.Session.GetObjectFromJson<List<Article>>("Panier") ?? new List<Article>();
            return View(panier);
        }
    }
}