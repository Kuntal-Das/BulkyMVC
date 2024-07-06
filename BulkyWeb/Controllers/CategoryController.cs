using Bulky.DataAccess.Repositories.Innterfaces;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name) && category.Name.Contains("test", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Name", "Test is an Invalid value");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = $"Category {category.Name} with Display Order {category.DisplayOrder} created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue || id is null || id < 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.Get(c => c.CategoryId == id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = $"Category {category.Name} with Display Order {category.DisplayOrder} updated Successfully";
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
            var category = _unitOfWork.Category.Get(c => c.CategoryId == id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            var tCat = _unitOfWork.Category.Get(c => c.CategoryId == category.CategoryId);
            _unitOfWork.Category.Remove(tCat);
            _unitOfWork.Save();
            TempData["error"] = $"Category {tCat.Name} with Display Order {tCat.DisplayOrder} Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
