using Amizades.Data;
using Amizades.Models;
using Amizades.Services;
using Amizades.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Security.Claims;

namespace Amizades.Controllers.Api
{
    [Route("api/[controller]")]
    public class PublicationController : ControllerBase
    {
        private int pageSize = 0;
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

        [HttpGet()]
        public IActionResult GetPublications(int page = 1)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var suggestions = _publicationService.GetPublicationWithPagination(userId, pageSize, page);

            /*ViewData["HasNextPage"] = suggestions.HasNextPage;*/

            return Ok()/*PartialView("~/Views/Home/Shared/_UserList.cshtml", suggestions.Items)*/;
        }
    }
}
