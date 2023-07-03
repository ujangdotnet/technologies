using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
	public class Category : Controller
	{
		
		private CategoryService categoryService;
		private int IdUser = 1;

        public Category(CategoryService _categoryService)
        {
            this.categoryService = _categoryService;
        }

        public async Task<ActionResult> Index(string sortOrder,
											  string searchString,
											  string currentFilter,
											  int? pageNumber,
											  int? pageSize)
		{
			ViewBag.Currentsort = sortOrder;
			ViewBag.CurrentPageSize = pageSize;
			ViewBag.NameSort = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

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

	}
}
