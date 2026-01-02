using GestionUsersMVC.Data;
using GestionUsersMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionUsersMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // INSCRIPTION
        // =========================
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user, string ConfirmPassword)
        {
            if (user.MotDePasse != ConfirmPassword)
            {
                ViewBag.Error = "Les mots de passe ne correspondent pas";
                return View(user);
            }

            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ViewBag.Error = "Cet email existe déjà";
                return View(user);
            }

            // ⚠️ L'admin reste Etudiant (comme tu veux)
            user.Role = Role.Etudiant;

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // =========================
        // CONNEXION
        // =========================
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string motDePasse)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email && u.MotDePasse == motDePasse);

            if (user == null)
            {
                ViewBag.Error = "Email ou mot de passe incorrect";
                return View();
            }

            // ✅ SESSIONS
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            return RedirectToAction("Index", "Home");
        }

        // =========================
        // DÉCONNEXION
        // =========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
