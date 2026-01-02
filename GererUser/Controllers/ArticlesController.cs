using GestionUsersMVC.Data;
using GestionUsersMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScienceStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GestionUsersMVC.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticlesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private List<string> GetDomaineList()
        {
            return new List<string> { "Physique", "Chimie", "Biologie", "Astronomie", "Robotique", "Informatique" };
        }

        public async Task<IActionResult> IndexM(string q, string domaine)
        {
            ViewBag.Domaines = await _context.Articles
                .Select(a => a.Domaine)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            var query = _context.Articles.AsQueryable();

            if (!string.IsNullOrEmpty(q))
                query = query.Where(a => a.Titre.Contains(q) || a.DescriptionCourte.Contains(q));

            if (!string.IsNullOrEmpty(domaine))
                query = query.Where(a => a.Domaine == domaine);

            var articles = await query.ToListAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ArticlesGrid", articles);
            }

            return View(articles);
        }

        public IActionResult Create()
        {
            ViewBag.DomaineChoices = new SelectList(GetDomaineList());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? uniqueFileName = null;
                if (model.FichierImage != null)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FichierImage.FileName;
                    string imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(imagesFolder)) Directory.CreateDirectory(imagesFolder);

                    string filePath = Path.Combine(imagesFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FichierImage.CopyToAsync(stream);
                    }
                }

                var article = new Article
                {
                    Titre = model.Nom,
                    Prix = model.Prix,
                    DescriptionCourte = model.DescriptionCourte ?? "Pas de description courte.",
                    NoteMoyenne = model.NoteMoyenne,
                    NomFichierImage = uniqueFileName,
                    // SÉCURITÉ : Empêche l'erreur SQL NULL
                    Description = model.Description ?? "Détails à venir",
                    Domaine = model.Domaine ?? "Général",
                    Annee = DateTime.Now.Year
                };

                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexM));
            }
            ViewBag.DomaineChoices = new SelectList(GetDomaineList());
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            var model = new ArticleViewModel
            {
                Id = article.Id,
                Nom = article.Titre,
                Prix = article.Prix,
                DescriptionCourte = article.DescriptionCourte,
                Description = article.Description,
                Domaine = article.Domaine,
                NomFichierImageExistant = article.NomFichierImage,
                NoteMoyenne = article.NoteMoyenne
            };

            ViewBag.DomaineChoices = new SelectList(GetDomaineList(), article.Domaine);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticleViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var article = await _context.Articles.FindAsync(id);
                if (article == null) return NotFound();

                article.Titre = model.Nom;
                article.Prix = model.Prix;
                article.DescriptionCourte = model.DescriptionCourte ?? "Pas de description courte.";

                // SÉCURITÉ : Empêche l'erreur SQL NULL lors de l'Update
                article.Description = model.Description ?? "Détails à venir";

                article.Domaine = model.Domaine ?? "Général";

                if (model.FichierImage != null)
                {
                    if (!string.IsNullOrEmpty(article.NomFichierImage))
                    {
                        string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", article.NomFichierImage);
                        if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FichierImage.FileName;
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FichierImage.CopyToAsync(stream);
                    }
                    article.NomFichierImage = uniqueFileName;
                }

                _context.Update(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexM));
            }
            ViewBag.DomaineChoices = new SelectList(GetDomaineList(), model.Domaine);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                if (!string.IsNullOrEmpty(article.NomFichierImage))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", article.NomFichierImage);
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                }
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(IndexM));
        }
    }
}