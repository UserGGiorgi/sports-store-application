using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStoreRepository repository;

        public HomeController(IStoreRepository repository)
        {
            this.repository = repository;
        }

        public int PageSize = 4;

        public ViewResult Index(string? category, int productPage = 1)
              => View(new ProductsListViewModel
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
