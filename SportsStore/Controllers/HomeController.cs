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

        public ViewResult Index(int productPage = 1)
        {
            return this.View(new ProductsListViewModel
            {
                Products = this.repository.Products
                               .OrderBy(p => p.ProductId)
                               .Skip((productPage - 1) * this.PageSize)
                               .Take(this.PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = this.PageSize,
                    TotalItems = this.repository.Products.Count(),
                },
            });
        }
    }
}
