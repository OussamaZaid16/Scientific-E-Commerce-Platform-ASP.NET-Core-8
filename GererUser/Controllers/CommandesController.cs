using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; // Nécessaire pour .Include()
using GestionUsersMVC.Models;
using GestionUsersMVC.Data;
using GestionUsersMVC.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionUsersMVC.Controllers
{
    public class CommandesController : Controller
    {
        private readonly AppDbContext _context;

        public CommandesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Affiche la page de saisie des coordonnées
        public IActionResult Checkout()
        {
            return View();
        }

        // 2. Traite l'enregistrement de la commande ET des articles
        [HttpPost]
        public async Task<IActionResult> Confirmer(Commande maCommande)
        {
            // Récupération du panier depuis la session
            var panier = HttpContext.Session.GetObjectFromJson<List<Article>>("Panier") ?? new List<Article>();

            if (panier.Count == 0) return RedirectToAction("Index", "Home");

            // --- CRUCIAL : Lier les articles du panier à la commande ---
            var articleIds = panier.Select(p => p.Id).ToList();

            // On récupère les vrais objets "Article" de la base pour la relation
            var articlesEnBase = await _context.Articles
                .Where(a => articleIds.Contains(a.Id))
                .ToListAsync();

            maCommande.Articles = articlesEnBase;
            maCommande.Total = (double)panier.Sum(x => x.Prix);
            maCommande.DateCommande = DateTime.Now;

            // Sauvegarde
            _context.Commandes.Add(maCommande);
            await _context.SaveChangesAsync();

            // Nettoyage
            HttpContext.Session.Remove("Panier");
            return View("Success");
        }

        // 3. Liste des commandes pour l'Admin avec inclusion des articles
        public async Task<IActionResult> Index()
        {
            // .Include(c => c.Articles) permet d'afficher les noms des produits dans la vue
            var liste = await _context.Commandes
                .Include(c => c.Articles)
                .OrderByDescending(c => c.DateCommande)
                .ToListAsync();

            return View(liste);
        }

        // 4. Action "À livrer" (Suppression/Archivage)
        public async Task<IActionResult> ALivrer(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);

            if (commande != null)
            {
                _context.Commandes.Remove(commande);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}