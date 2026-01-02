using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using GestionUsersMVC.Data;
using GestionUsersMVC.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionUsersMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Index : Gère la sécurité, le filtrage et la pagination (6 articles par page)
        public async Task<IActionResult> Index(string q, string domaine, int page = 1)
        {
            // 1. SÉCURITÉ : Vérification de la session
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. PARAMÈTRES DE PAGINATION
            int pageSize = 6;

            // 3. PRÉPARATION DE LA REQUÊTE
            var query = _context.Articles.AsQueryable();

            // Filtrage par texte
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(a => a.Titre.Contains(q));
            }

            // Filtrage par domaine
            if (!string.IsNullOrEmpty(domaine))
            {
                query = query.Where(a => a.Domaine == domaine);
            }

            // 4. CALCULS POUR LA PAGINATION
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Sécurité pour ne pas dépasser les pages existantes
            if (page < 1) page = 1;

            // Récupération des articles pour la page actuelle
            var articles = await query
                .OrderBy(a => a.Titre) // Important pour une pagination stable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 5. PRÉPARATION DES DONNÉES POUR LA VUE
            ViewBag.Domaines = await _context.Articles
                .Select(a => a.Domaine)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentSearch = q;
            ViewBag.SelectedDomaine = domaine;

            // 6. GESTION DU RETOUR (AJAX ou Vue complète)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ArticlesGrid", articles);
            }

            return View(articles);
        }

        // Page de détails d'un article
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            if (id == null) return NotFound();

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.Id == id);

            if (article == null) return NotFound();

            return View(article);
        }

        public IActionResult Contact() => View();
        public IActionResult About() => View();
        public IActionResult Privacy() => View();
    }
}