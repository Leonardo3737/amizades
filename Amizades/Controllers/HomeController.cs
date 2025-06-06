using System.Diagnostics;
using System.Drawing.Printing;
using System.Security.Claims;
using Amizades.Data;
using Amizades.Models;
using Amizades.Models.Enums;
using Amizades.Services;
using Amizades.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Amizades.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HomeService _homeService { get; set; }

        public HomeController(
            ILogger<HomeController> logger,
            HomeService homeService
        )
        {
            _logger = logger;
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var data = _homeService.GetHomeViewModelData(userId);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
