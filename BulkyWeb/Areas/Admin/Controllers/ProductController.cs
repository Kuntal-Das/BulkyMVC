using Bulky.DataAccess.Repositories.Innterfaces;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll();
            return View(products);
        }
        public IActionResult Create()
        {
            var pvm = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem(c.Name, c.CategoryId.ToString()))
            };
            return View(pvm);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVm)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVm.Product);
                _unitOfWork.Save();
                TempData["success"] = $"Book {productVm.Product.Title} of {productVm.Product.Author} created Successfully";
                return RedirectToAction("Index");
            }

            productVm.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem(c.Name, c.CategoryId.ToString()));
            return View(productVm);
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue || id is null || id < 0)
            {
                return NotFound();
            }
            var product = _unitOfWork.Product.Get(c => c.Id == id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData["success"] = $"Book {product.Title} of {product.Author} updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (!id.HasValue || id is null || id < 0)
            {
                return NotFound();
            }
            var product = _unitOfWork.Product.Get(c => c.Id == id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            var tProd = _unitOfWork.Product.Get(c => c.Id == product.Id);
            _unitOfWork.Product.Remove(tProd);
            _unitOfWork.Save();
            TempData["error"] = $"Category {tProd.Title} of {tProd.Author} Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
