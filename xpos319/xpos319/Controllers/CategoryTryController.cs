using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
	public class CategoryTryController : Controller
	{
		private readonly XPOS_319Context db;
		private readonly CategoryTryService categoryTryService;

		public CategoryTryController(XPOS_319Context _db)
		{
			this.db = _db;
			this.categoryTryService = new CategoryTryService(db);
		}

		public IActionResult Index()
		{
			List<VMTblCategory> dataView = categoryTryService.GetAllData();
			return View(dataView);
		}

		public IActionResult Create()
		{
			VMTblCategory dataView = new VMTblCategory();

			return View(dataView);
		}
	}
}
