using Microsoft.AspNetCore.Mvc;

namespace Amizades.ViewModels
{
    public class PaginationViewModel<T>
    {
        public bool HasNextPage { get; set; }
        public List<T> Items { get; set; }
        public PartialViewResult? HtmlText { get; set; }
    }
}
