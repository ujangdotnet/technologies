using System.Drawing.Text;
using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
    public class RoleController : Controller
    {
        private RoleService roleService;
        private int IdUser = 1;

        public RoleController(RoleService _roleService)
        {
            this.roleService = _roleService;
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

            List<TblRole> data = await roleService.GetAllData();


            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(a => a.RoleName.ToLower().Contains(searchString.ToLower())).ToList();

            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(a => a.RoleName).ToList();
                    break;

                default:
                    data = data.OrderBy(a => a.RoleName).ToList();
                    break;
            }

            return View(PagInatedList<TblRole>.CreateAsync(data, pageNumber ?? 1, pageSize ?? 3));
        }

        public async Task<IActionResult> Index_MenuAccess(
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

            List<TblRole> data = await roleService.GetAllData();


            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(a => a.RoleName.ToLower().Contains(searchString.ToLower())).ToList();

            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(a => a.RoleName).ToList();
                    break;

                default:
                    data = data.OrderBy(a => a.RoleName).ToList();
                    break;
            }

            return View(PagInatedList<TblRole>.CreateAsync(data, pageNumber ?? 1, pageSize ?? 3));
        }
        public IActionResult Create()
        {
            TblRole data = new TblRole();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Creates(TblRole dataParam)
        {
            dataParam.CreateBy = IdUser;
            VMResponse response = await roleService.Create(dataParam);

            if (response.Success)
            {
                return Json(new { dataResponse = response });
            }
            return View(dataParam);
        }

        public async Task<JsonResult> CheckNameIsExist(string name, int Id)
        {
            bool isExist = await roleService.CheckRoleByName(name, Id);
            return Json(isExist);
        }

        public async Task<IActionResult> Detail(int id)
        {
            VMTblRole data = await roleService.GetDataById(id);
            return View(data);
        }

        public async Task<IActionResult> Edit(int id)
        {
            VMTblRole data = await roleService.GetDataById(id);
            return View(data);
        }

        public async Task<IActionResult> Edit_MenuAccess(int id)
        {
            VMTblRole data = await roleService.GetDataById(id);
            ViewBag.RoleMenu = data.RoleName;
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TblRole dataParam)
        {
            dataParam.UpdateBy = IdUser;
            VMResponse respon = await roleService.Edit(dataParam);

            if (respon.Success)
            {
                return Json(new { dataResponse = respon });
            }
            return View(dataParam);
        }

        public async Task<IActionResult> Delete(int id)
        {
            VMTblRole data = await roleService.GetDataById(id);
            return View(data);
        }

        public async Task<IActionResult> SureDelete(int id)
        {

            int createBy = IdUser;
            VMResponse respon = await roleService.Delete(id, createBy);

            if (respon.Success)
            {
                //return RedirectToAction("Index");
                return Json(new { dataResponse = respon });
            }
            return RedirectToAction("Index");

        }


    }
}

