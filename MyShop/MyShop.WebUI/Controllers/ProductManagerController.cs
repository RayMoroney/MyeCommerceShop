using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.DataAccess.InMemory;
using MyShop.Core.Models;
using System.Data.Entity;
using MyShop.Core.ViewModels;
using MyShop.Core.Contracts;
using System.IO;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<ProductModel> context;
        IRepository<ProductCategoryModel> productCategories;

        public ProductManagerController(IRepository<ProductModel> productContext, IRepository<ProductCategoryModel> productCategoriesContext)
        {
            context = productContext;
            productCategories = productCategoriesContext;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<ProductModel> products = context.Collection().ToList();

            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            viewModel.Product = new ProductModel();
            viewModel.ProductCategories = productCategories.Collection();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(ProductModel product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult Edit(string id)
        {
            ProductModel product = context.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();

                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductModel product, string id, HttpPostedFileBase file)
        {
            ProductModel productToEdit = context.Find(id);

            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                    }

                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Name = product.Name;
                    productToEdit.Price = product.Price;

                    context.Update(productToEdit);
                    context.Commit();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(product);
                }
            }
        }

        public ActionResult Delete(string id)
        {
            ProductModel product = context.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            ProductModel productToDelete = context.Find(id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(id);
                context.Commit();

                return RedirectToAction("Index");
            }
        }
    }
}