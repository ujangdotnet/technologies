using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
    public class CustomerController : Controller
    {
        private readonly RoleService roleService;
        private readonly CustomerService customerService;

        public CustomerController(RoleService _roleService, CustomerService _customerService)
        {
            this.roleService = _roleService;
            this.customerService = _customerService;
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

            List<VMTblCustomer> data = await customerService.GetAllData();

            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(a => a.NameCustomer.ToLower().Contains(searchString.ToLower())
                || a.RoleName.ToLower().Contains(searchString.ToLower())).ToList();

            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(a => a.NameCustomer).ToList();
                    break;
                default:
                    data = data.OrderBy(a => a.NameCustomer).ToList();
                    break;
            }

            return View(PagInatedList<VMTblCustomer>.CreateAsync(data, pageNumber ?? 1, pageSize ?? 3));
        }

        public async Task<IActionResult> Create()
        {
            VMTblCustomer data = new VMTblCustomer();
            List<TblRole> roleList = await roleService.GetAllData();
            ViewBag.RoleList = roleList;

            return View(data);
        }
    }
}
