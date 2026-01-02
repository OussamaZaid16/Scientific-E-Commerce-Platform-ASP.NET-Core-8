using GestionUsersMVC.Data;
using GestionUsersMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionUsersMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // 🔐 Vérification admin par email
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserEmail") == "admin@gmail.com";
        }

        // =========================
        // LISTE DES UTILISATEURS
        // =========================
        public IActionResult Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var users = _context.Users.ToList();
            return View(users);
        }

        // =========================
        // CREATE
        // =========================
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // =========================
        // EDIT
        // =========================
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            // Récupérer l'utilisateur existant depuis la base
            var existingUser = _context.Users.Find(user.Id);
            if (existingUser == null)
                return NotFound();

            // Mettre à jour les champs sauf le mot de passe
            existingUser.Nom = user.Nom;
            existingUser.Prenom = user.Prenom;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;

            // Mettre à jour le mot de passe seulement si l'utilisateur en a fourni un
            if (!string.IsNullOrEmpty(user.MotDePasse))
            {
                existingUser.MotDePasse = user.MotDePasse;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // =========================
        // DELETE
        // =========================
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
