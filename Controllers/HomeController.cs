using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.Repository;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly int pageSize = 4;
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
        {
            var productsQuery = this.repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductId)
                .Skip((productPage - 1) * this.pageSize)
                .Take(this.pageSize);

            var totalItems = category == null
                ? this.repository.Products.Count()
                : this.repository.Products.Count(p => p.Category == category);

            return this.View(new ProductsListViewModel
            {
                Products = productsQuery,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = this.pageSize,
                    TotalItems = totalItems,
                },
                CurrentCategory = category,
            });
        }
    }
}
