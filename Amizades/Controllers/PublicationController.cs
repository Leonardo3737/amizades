using Amizades.Data;
using Amizades.Models;
using Amizades.Services;
using Amizades.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Security.Claims;

namespace Amizades.Controllers
{
    public class PublicationController : Controller
    {
        private AmizadesContext _context { get; set; }
        private PublicationService _publicationService { get; set; }

        public PublicationController(
            AmizadesContext context,
            PublicationService publicationService
            )
        {
            _context = context;
            _publicationService = publicationService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreatePublicationViewModel publication)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var newPublication = new Publication()
            {
                AuthorId = userId,
                PublicationText = publication.PublicationText,
            };
            _context.Publications.Add(newPublication);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
