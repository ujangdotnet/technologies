using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService productService;
        private readonly CategoryService categoryService;
        private readonly VariantService variantService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private int IdUser = 1;

        public ProductController(VariantService _variantService, CategoryService _categoryService, ProductService _productService, IWebHostEnvironment _webHostEnvironment)
        {
            this.variantService = _variantService;
            this.categoryService = _categoryService;
            this.productService = _productService;
            this.webHostEnvironment = _webHostEnvironment;
        }

        public async Task<IActionResult> Index(
                                               string sortOrder,
                                               string searchString,
                                               string currentFilter,
                                               int? pageNumber,
                                               int? pageSize)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentPageSize = pageSize;
            ViewBag.NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSort = sortOrder == "price" ? "price_desc" : "price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            List<VMTblProduct> data = await productService.GetAllData();

            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(a => a.NameProduct.ToLower().Contains(searchString.ToLower())
                || a.NameCategory.ToLower().Contains(searchString.ToLower())
                || a.NameVariant.ToLower().Contains(searchString.ToLower())).ToList();

            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(a => a.NameProduct).ToList();
                    break;
                case "price_desc":
                    data = data.OrderByDescending(a => a.Price).ToList();
                    break;
                case "price":
                    data = data.OrderBy(a => a.Price).ToList();
                    break;
                default:
                    data = data.OrderBy(a => a.NameVariant).ToList();
                    break;
            }

            return View(PagInatedList<VMTblProduct>.CreateAsync(data, pageNumber ?? 1, pageSize ?? 3));
        }

        public async Task<IActionResult> Create()
        {
            VMTblProduct data = new VMTblProduct();
            List<VMTblVariant> variantList = await variantService.GetAllData();
            List<TblCategory> categoryList = await categoryService.GetAllData();
            ViewBag.VariantList = variantList;
            ViewBag.CategoryList = categoryList;

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VMTblProduct dataParam)
        {
            if(dataParam.ImageFile != null)
            {
                dataParam.Image = Upload(dataParam);
            }

            VMResponse respon = await productService.Create(dataParam);

            if (respon.Success)
            {
                return Json(new { dataRespon = respon });
            }

            return View(dataParam);
        }

        public async Task<JsonResult> CheckByName(string name, int id, int idVariant)
        {
            bool isExist = await productService.CheckByName(name, id, idVariant);
            return Json(isExist);
        }

        public string Upload(VMTblProduct dataParam)
        {
            string uniqueFileName = "";

            if (dataParam.ImageFile != null)
            {
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + dataParam.ImageFile.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    dataParam.ImageFile.CopyTo(fileStream);
                }
            }


            return uniqueFileName;
        }
    }
}
