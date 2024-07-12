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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: nameof(Product.Category));
            return View(products);
        }
        public IActionResult Upsert(int? id)
        {
            var pvm = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem(c.Name, c.CategoryId.ToString()))
            };
            if (id.HasValue && id is not null && id != 0)
            {
                pvm.Product = _unitOfWork.Product.Get(p => p.Id == id.Value) ?? new Product();

                return View(pvm);
            }
            else
            {
                pvm.Product = new Product();
            }
            return View(pvm);
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVm, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                if (imageFile is not null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string productImageDir = Path.Combine(wwwrootPath, @"images/Product");
                    if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                    {
                        var oldImgPath = Path.Combine(wwwrootPath, productVm.Product.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }

                    Directory.CreateDirectory(productImageDir);
                    using (var fs = new FileStream(Path.Combine(productImageDir, fileName), FileMode.Create))
                    {
                        imageFile.CopyTo(fs);
                    }
                    productVm.Product.ImageUrl = @"/images/product/" + fileName;
                }

                if (productVm.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVm.Product);
                }
                else
                {

                    _unitOfWork.Product.Update(productVm.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = $"Book {productVm.Product.Title} of {productVm.Product.Author} created Successfully";
                return RedirectToAction("Index");
            }

            productVm.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem(c.Name, c.CategoryId.ToString()));
            return View(productVm);
        }

        //public IActionResult Delete(int? id)
        //{
        //    if (!id.HasValue || id is null || id < 0)
        //    {
        //        return NotFound();
        //    }
        //    var product = _unitOfWork.Product.Get(c => c.Id == id.Value);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}
        //[HttpPost]
        //public IActionResult Delete(Product product)
        //{
        //    var tProd = _unitOfWork.Product.Get(c => c.Id == product.Id);
        //    if (tProd == null) return NotFound();

        //    _unitOfWork.Product.Remove(tProd);
        //    _unitOfWork.Save();
        //    TempData["error"] = $"Category {tProd.Title} of {tProd.Author} Deleted Successfully";
        //    return RedirectToAction("Index");
        //}

        #region API
        [HttpGet]
        public IActionResult All()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: nameof(Product.Category));
            return Json(new { data = products });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null) return Json(new { success = "false", message = "No Id provided" });
            var productToDel = _unitOfWork.Product.Get(p => p.Id == id);
            if (productToDel is null)
            {
                return Json(new { success = "false", message = "No Product Found" });
            }

            string wwwrootPath = _webHostEnvironment.WebRootPath;
            string productImageDir = Path.Combine(wwwrootPath, @"images/Product");
            if (!string.IsNullOrEmpty(productToDel.ImageUrl))
            {
                var oldImgPath = Path.Combine(wwwrootPath, productToDel.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImgPath))
                {
                    System.IO.File.Delete(oldImgPath);
                }
            }
            _unitOfWork.Product.Remove(productToDel);
            _unitOfWork.Save();

            return Json(new { success = "true", message = $"Category {productToDel.Title} of {productToDel.Author} Deleted Successfully" });
        }
        #endregion
    }
}
