using Amizades.Data;
using Amizades.ViewModels;
using Amizades.Models;
using Amizades.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace Amizades.Controllers
{
    public class AccountController : Controller
    {
        private AmizadesContext _context { get; set; }
        private JwtService _jwtService { get; set; }
        private EncryptionService _encryptionService { get; set; }
        public AccountController(
            AmizadesContext context, 
            JwtService jwtService,
            EncryptionService encryptionService
            )
        {
            _context = context;
            _jwtService = jwtService;
            _encryptionService = encryptionService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            // Exemplo: verificação simples
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Credenciais inválidas");
                return View(dto);
            }

            var isCorrectPassword = _encryptionService.Verify<User>(dto.Password, user.Password);

            if(!isCorrectPassword)
            {
                ModelState.AddModelError("", "Senha Incorreta.");
                return View(dto);
            }

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email);

            // Armazena o token no cookie (ou localStorage, se for SPA)
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(User newUser)
        {
            Console.WriteLine("cadastrando");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Invalido");
                return View(newUser);
            }

            try
            {
                newUser.Password = _encryptionService.Encrypt<User>(newUser.Password);
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
            }
            catch
            {
                Console.WriteLine("erro");
                ModelState.AddModelError("", "Ocorreu um erro ao cadastrar novo usuário");
                return View(newUser);
            }
            Console.WriteLine("cadastrou");
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login");
        }
    }
}
