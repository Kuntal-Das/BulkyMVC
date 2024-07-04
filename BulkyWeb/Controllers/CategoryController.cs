using Bulky.DataAccess.Repositories.Innterfaces;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository db)
        {
            _categoryRepository = db;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
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
                _categoryRepository.Add(category);
                _categoryRepository.Save();
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
            var category = _categoryRepository.Get(c => c.CategoryId == id.Value);
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
                _categoryRepository.Update(category);
                _categoryRepository.Save();
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
            var category = _categoryRepository.Get(c => c.CategoryId == id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            var tCat = _categoryRepository.Get(c => c.CategoryId == category.CategoryId);
            _categoryRepository.Remove(tCat);
            _categoryRepository.Save();
            TempData["error"] = $"Category {tCat.Name} with Display Order {tCat.DisplayOrder} Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
