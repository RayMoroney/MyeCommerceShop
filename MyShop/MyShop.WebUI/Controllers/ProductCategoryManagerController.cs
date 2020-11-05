using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductCategoryManagerController : Controller
    {
        IRepository<ProductCategoryModel> context;

        public ProductCategoryManagerController(IRepository<ProductCategoryModel> productCategoryModel)
        {
            context = productCategoryModel;
        }

        // GET: ProductCategoryManager
        public ActionResult Index()
        {
            List<ProductCategoryModel> categories = context.Collection().ToList();

            return View(categories);
        }

        public ActionResult Create()
        {
            ProductCategoryModel category = new ProductCategoryModel();

            return View(category);
        }

        [HttpPost]
        public ActionResult Create(ProductCategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                context.Insert(categoryModel);
                context.Commit();

                return RedirectToAction("Index");
            }
            else
            {
                return View(categoryModel);
            }
        }

        public ActionResult Edit(string id)
        {
            ProductCategoryModel category = context.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategoryModel categoryModel, string id)
        {
            ProductCategoryModel categoryToEdit = context.Find(id);

            if (categoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    categoryToEdit.Category = categoryModel.Category;

                    context.Update(categoryToEdit);
                    context.Commit();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(categoryModel);
                }
            }
        }

        public ActionResult Delete(string id)
        {
            ProductCategoryModel category = context.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            ProductCategoryModel categoryToDelete = context.Find(id);

            if (categoryToDelete == null)
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