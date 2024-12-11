﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.Repository;

namespace SportsStore.Controllers
{
    [Authorize]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private IStoreRepository storeRepository;
        private IOrderRepository orderRepository;

        [Route("Details/{productId:int}")]
        public ViewResult Details(int productId)
            => View(storeRepository.Products.FirstOrDefault(p => p.ProductId == productId));

        public AdminController(IStoreRepository storeRepository, IOrderRepository orderRepository)
            => (this.storeRepository, this.orderRepository) = (storeRepository, orderRepository);

        [Route("Orders")]
        public ViewResult Orders() => this.View(this.orderRepository.Orders);

        [Route("Products")]
        public ViewResult Products() => this.View(this.storeRepository.Products);

        [HttpPost]
        [Route("MarkShipped")]
        public IActionResult MarkShipped(int orderId)
        {
            Order? order = this.orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (order != null)
            {
                order.Shipped = true;
                this.orderRepository.SaveOrder(order);
            }

            return this.RedirectToAction("Orders");
        }

        [HttpPost]
        [Route("Reset")]
        public IActionResult Reset(int orderId)
        {
            Order? order = this.orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (order != null)
            {
                order.Shipped = false;
                this.orderRepository.SaveOrder(order);
            }

            return this.RedirectToAction("Orders");
        }

        [Route("Products/Edit/{productId:long}")]
        public ViewResult Edit(int productId)
        {
            return View(storeRepository.Products.FirstOrDefault(p => p.ProductId == productId));
        }

        [HttpPost]
        [Route("Products/Edit/{productId:long}")]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                storeRepository.SaveProduct(product);
                return RedirectToAction("Products");
            }

            return View(product);
        }

        [Route("Products/Create")]
        public ViewResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        [Route("Products/Create")]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                storeRepository.SaveProduct(product);
                return RedirectToAction("Products");
            }

            return View(product);
        }

        [Route("Products/Delete/{productId:long}")]
        public IActionResult Delete(int productId)
            => View(storeRepository.Products.FirstOrDefault(p => p.ProductId == productId));

        [HttpPost]
        [Route("Products/Delete/{productId:long}")]
        public IActionResult DeleteProduct(int productId)
        {
            var product = storeRepository.Products.FirstOrDefault(p => p.ProductId == productId);
            storeRepository.DeleteProduct(product);
            return RedirectToAction("Products");
        }
    }
}