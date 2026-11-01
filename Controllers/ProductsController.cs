using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsValidation.Models;
using ProductsValidation.Services;

namespace ProductsValidation.Controllers
{
    public class ProductsController : Controller
    {
        private List<Product> myProducts;

        public ProductsController(Data data)
        {
            myProducts = data.Products;
        }

        public IActionResult Index()
        {
            return View(myProducts);
        }

        public IActionResult View(int id)
        {
            Product prod = myProducts.Find(p => p.Id == id);
            if (prod != null)
            {
                return View(prod);
            }

            return View("NotExists");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product prod = myProducts.Find(p => p.Id == id);
            if (prod != null)
            {
                return View(prod);
            }

            return View("NotExists");
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            int index = myProducts.FindIndex(p => p.Id == product.Id);
            if (index < 0)
            {
                return View("NotExists");
            }

            if (!ModelState.IsValid)
            {
                return View(product); // помилки на тій самій сторінці
            }

            myProducts[index] = product;
            return RedirectToAction("View", new { id = product.Id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Product() { Id = NextId() });
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            product.Id = NextId();
            ModelState.Remove(nameof(Product.Id));

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            myProducts.Add(product);
            return RedirectToAction("View", new { id = product.Id });
        }

        public IActionResult Delete(int id)
        {
            Product prod = myProducts.Find(p => p.Id == id);
            if (prod != null)
            {
                myProducts.Remove(prod);
            }

            return RedirectToAction("Index");
        }

        // ---------- Редагування цін усієї категорії ----------

        [HttpGet]
        public IActionResult EditPrices(Product.Category category)
        {
            if (!ModelState.IsValid || !Enum.IsDefined(typeof(Product.Category), category))
            {
                return RedirectToAction("Index");
            }

            var model = new PricesEditViewModel
            {
                Category = category,
                Items = myProducts
                    .Where(p => p.Type == category)
                    .Select(p => new ProductPriceItem { Id = p.Id, Name = p.Name, Price = p.Price })
                    .ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditPrices(PricesEditViewModel model)
        {
            if (!Enum.IsDefined(typeof(Product.Category), model.Category))
            {
                return RedirectToAction("Index");
            }

            var products = myProducts.Where(p => p.Type == model.Category).ToDictionary(p => p.Id);
            var items = model.Items ?? new List<ProductPriceItem>();

            // захист від підміни Id: усі рядки мають належати обраній категорії
            if (items.Any(i => !products.ContainsKey(i.Id)))
            {
                return BadRequest();
            }

            // Name не береться з форми, підставляємо з джерела даних
            foreach (var item in items)
            {
                item.Name = products[item.Id].Name;
            }
            model.Items = items;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            foreach (var item in items)
            {
                products[item.Id].Price = item.Price;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private int NextId()
        {
            return myProducts.Any() ? myProducts.Max(p => p.Id) + 1 : 1;
        }
    }
}