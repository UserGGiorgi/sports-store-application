using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        public int PageSize = 4;
        private readonly IStoreRepository repository;

        public HomeController(IStoreRepository repository)
        {
            this.repository = repository;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View();
        }

        public ViewResult Index(string? category, int productPage = 1)
              => this.View(new ProductsListViewModel
              {
                  Products = this.repository.Products
                .Where(p => category == null || p.Category == category)
                  .OrderBy(p => p.ProductId)
                  .Skip((productPage - 1) * this.PageSize)
                  .Take(this.PageSize),
                  PagingInfo = new PagingInfo
                  {
                      CurrentPage = productPage,
                      ItemsPerPage = this.PageSize,
                      TotalItems = category == null ? this.repository.Products.Count() : this.repository.Products.Where(e => e.Category == category).Count(),
                  },
                  CurrentCategory = category,
              });
    }
}
