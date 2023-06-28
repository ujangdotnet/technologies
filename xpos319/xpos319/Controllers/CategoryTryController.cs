using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.Models;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
	public class CategoryTryController : Controller
	{
		private readonly XPOS_319Context db;
		private readonly CategoryTryService categoryTryService;

		public CategoryTryController(XPOS_319Context _db) //constructor ini buat apa ?
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

		[HttpPost]
		public IActionResult Create(VMTblCategory dataView)
		{
			VMResponse respon = new VMResponse();

			if(ModelState.IsValid)
			{
				respon = categoryTryService.Create(dataView);

				if (respon.Success)
				{
					return RedirectToAction("Index");
				}
			}

			respon.Entity = dataView;
			return View(respon.Entity);
		}

		public IActionResult Edit(int id)
		{
			VMTblCategory dataView = categoryTryService.GetById(id);
			return View(dataView);
		}

		[HttpPost]
		public IActionResult Edit(VMTblCategory dataView)
		{
            VMResponse respon = new VMResponse();

            if (ModelState.IsValid)
            {
                respon = categoryTryService.Edit(dataView);

                if (respon.Success)
                {
                    return RedirectToAction("Index");
                }
            }

            respon.Entity = dataView;
            return View(respon.Entity);
        }

		//detail
		public IActionResult Detail(int id)
		{
            VMTblCategory dataView = categoryTryService.GetById(id);
            return View(dataView);
        }

        public IActionResult Delete(int id)
        {
            VMTblCategory dataView = categoryTryService.GetById(id);
            return View(dataView);
        }

		[HttpPost]
		public IActionResult Delete(VMTblCategory dataView)
		{
			VMResponse respon = new VMResponse();
			respon = categoryTryService.Delete(dataView);

				if (respon.Success)
				{
					return RedirectToAction("Index");
				}

			return View(respon.Entity);
		}
	}
}
