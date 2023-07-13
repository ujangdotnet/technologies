using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
    public class VariantController : Controller
    {

        private VariantService variantService;
        private CategoryService categoryService;
        private int IdUser = 1;

        public VariantController(VariantService _variantService, CategoryService _categoryService)
        {
            this.variantService = _variantService;
            this.categoryService = _categoryService;
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

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            List<VMTblVariant> data = await variantService.GetAllData();

            var deskrip = data.Where(a => a.Description is null).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(a => a.NameCategory.ToLower().Contains(searchString.ToLower()) 
                || a.Description.ToLower().Contains(searchString.ToLower())
                || a.NameVariant.ToLower().Contains(searchString.ToLower())).ToList();

            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(a => a.NameVariant).ToList();
                    break;

                default:
                    data = data.OrderBy(a => a.NameVariant).ToList();
                    break;
            }

            return View(PagInatedList<VMTblVariant>.CreateAsync(data, pageNumber ?? 1, pageSize ?? 3));
        }

        public async Task<IActionResult> Create()
        {
            VMTblVariant data = new VMTblVariant();

            List<TblCategory> listCategory = await categoryService.GetAllData();
            ViewBag.ListCategory = listCategory;

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VMTblVariant dataParam)
        {
            dataParam.CreateBy = IdUser;

            VMResponse respon = await variantService.Create(dataParam);

            if (respon.Success)
            {
                return Json(new { dataRespon = respon });
            }

            return View(dataParam);
        }

        public async Task<JsonResult> CheckByName(string name, int id, int idCategory)
        {
            bool isExist = await variantService.CheckByName(name, id, idCategory);
            return Json(isExist);
        }

        public async Task<IActionResult> Delete(int id)
        {
            VMTblVariant data = await variantService.GetDataById(id);

            return View(data);
        }

        //tambahin suredelete

        public async Task<IActionResult> MultipleDelete(List<int> listId)
        {
            List<string> listName = new List<string>();
            foreach(int item in listId)
            {
                VMTblVariant data = await variantService.GetDataById(item);
                listName.Add(data.NameVariant);
            }

            ViewBag.ListName = listName;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> SureMultipleDelete(List<int> listId)
        {
            VMResponse respon = await variantService.MultipleDelete(listId);

            if (respon.Success)
            {
                return Json(new { dataRespon = respon });
            }

            return RedirectToAction("Index");
        }
    }
}
