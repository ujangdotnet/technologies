using System.Drawing.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using xpos319.datamodels;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
    public class VariantTryController : Controller
    {
        private readonly XPOS_319Context db;
        private readonly VariantTryService variantTryService;
        private readonly CategoryTryService categoryTryService;
        private static VMPage page = new VMPage();

        //membuat contructor untuk memanggil data
        public VariantTryController(XPOS_319Context _db)
        {
            this.db = _db;
            this.variantTryService = new VariantTryService(db);
            this.categoryTryService = new CategoryTryService(db);
        }
        public IActionResult Index(VMPage pg)
        {
            ViewBag.idSort = string.IsNullOrEmpty(pg.sortOrder) ? "id_desc" : "";
            ViewBag.nameSort = pg.sortOrder == "name" ? "name_desc" : "name";
            ViewBag.CurrentSort = pg.sortOrder;
            ViewBag.currentShowData = pg.showData;

            if(pg.searchString != null){
                pg.pageNumber = 1;
            }
            else
            {
                pg.searchString = pg.currentFilter;
            }

            ViewBag.CurrentFilter = pg.searchString;

            List<VMTblVariant> dataView = variantTryService.GetAllData();

            if (!string.IsNullOrEmpty(pg.searchString))
            {
                dataView = dataView.Where(a => a.NameVariant.ToLower().Contains(pg.searchString.ToLower())
                || a.NameCategory.ToLower().Contains(pg.searchString.ToLower())).ToList();
            }

            switch (pg.sortOrder)
            {
                case "name_desc":
                    dataView = dataView.OrderByDescending(a => a.NameVariant).ToList();
                    break;
                case "name":
                    dataView = dataView.OrderBy(a => a.NameCategory).ToList();
                    break;
                case " is_desc":
                    dataView = dataView.OrderByDescending(a => a.NameVariant).ToList();
                    break;
                default:
                    dataView = dataView.OrderBy(a => a.Id).ToList();
                    break;
            }

            int pageSize = pg.showData ?? 3;

            page = pg;
            return View(PagInatedList<VMTblVariant>.CreateAsync(dataView, pg.pageNumber ?? 1, pageSize));

          
        }

        //create
        public IActionResult Create()
        {
            VMTblVariant dataView = new VMTblVariant();
            ViewBag.dropDownCat = categoryTryService.GetAllData();
            return View(dataView);
        }

        [HttpPost]
        public IActionResult Create(VMTblVariant dataView)
        {
            VMResponse respon = new VMResponse();

            if(ModelState.IsValid)
            {
                respon = variantTryService.Create(dataView);

                if(respon.Success)
                {
                    return RedirectToAction("Index", page);
                }
            }

            ViewBag.dropDownCat = categoryTryService.GetAllData();
            respon.Entity = dataView;
            return View(respon.Entity);
        }
    }
}
