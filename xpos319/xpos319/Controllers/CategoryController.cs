using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
	public class CategoryController : Controller
	{
		
		private CategoryService categoryService;
		private int IdUser = 1;

        public CategoryController(CategoryService _categoryService)
        {
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

            List<TblCategory> data = await categoryService.GetAllData();

            var deskrip = data.Where(a => a.Description is null).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(a => a.NameCategory.ToLower().Contains(searchString.ToLower()) || a.Description.ToLower().Contains(searchString.ToLower())).ToList();

            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(a => a.NameCategory).ToList();
                    break;

                default:
                    data = data.OrderBy(a => a.NameCategory).ToList();
                    break;
            }

            return View(PagInatedList<TblCategory>.CreateAsync(data, pageNumber ?? 1, pageSize ?? 3));
        }

        public IActionResult Create()
		{
			TblCategory data = new TblCategory();
			return View(data);
		}

        [HttpPost]
        public async Task<IActionResult> Creates(TblCategory dataParam)
        {
            dataParam.CreateBy = IdUser;
            VMResponse response = await categoryService.Create(dataParam);

            if (response.Success)
            {
                return Json(new { dataResponse = response });
            }
            return View(dataParam);
        }

        public async Task<JsonResult> CheckNameIsExist(string nameCategory, int Id)
        {
            bool isExist = await categoryService.CheckCategoryByName(nameCategory, Id);
            return Json(isExist);
        }

        public async Task<IActionResult> Edit(int id)
        {
            TblCategory data = await categoryService.GetDataById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TblCategory dataParam)
        {
            dataParam.UpdateBy = IdUser;
            VMResponse respon = await categoryService.Edit(dataParam);

            if (respon.Success)
            {
                return Json(new { dataRespon = respon });
            }
            return View(dataParam);
        }

        public async Task<IActionResult> Detail(int id)
        {
            TblCategory data = await categoryService.GetDataById(id);
            return View(data);
        }

        public async Task<IActionResult> Delete(int id)
        {
            TblCategory data = await categoryService.GetDataById(id);
            return View(data);
        }

		public async Task<IActionResult> SureDelete(int id)
		{
            
            int createBy = IdUser;
            VMResponse respon = await categoryService.Delete(id, createBy);

            if (respon.Success)
            {
                //return RedirectToAction("Index");
                return Json(new { dataResponse = respon });
            }
            return RedirectToAction("Index");
        }
	}
}
