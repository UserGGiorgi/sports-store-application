using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.Repository;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IStoreRepository repository;

        public CartController(IStoreRepository repository, Cart cart)
        {
            this.repository = repository;
            this.Cart = cart;
        }

        public Cart Cart { get; set; }

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            return this.View(new CartViewModel
            {
                ReturnUrl = returnUrl ?? "/",
                Cart = this.Cart,
            });
        }

        [HttpPost]
        public IActionResult Index(long productId, string returnUrl)
        {
            Product? product = this.repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                this.Cart.AddItem(product, 1);

                return this.View(new CartViewModel
                {
                    Cart = this.Cart,
                    ReturnUrl = returnUrl,
                });
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("Cart/Remove")]
        public IActionResult Remove(long productId, string returnUrl)
        {
            this.Cart.RemoveLine(this.Cart.Lines.First(cl => cl.Product.ProductId == productId).Product);
            return this.View("Index", new CartViewModel
            {
                Cart = this.Cart,
                ReturnUrl = returnUrl ?? "/",
            });
        }
    }
}
