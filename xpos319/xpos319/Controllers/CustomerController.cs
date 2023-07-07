using Microsoft.AspNetCore.Mvc;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
    public class CustomerController : Controller
    {
        private readonly RoleService roleService;
        private readonly CustomerService customerService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CustomerController(RoleService _roleService, CustomerService _customerService, IWebHostEnvironment _webHostEnvironment)
        {
            this.roleService = _roleService;
            this.customerService = _customerService;
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
                || a.NameRole.ToLower().Contains(searchString.ToLower())).ToList();

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
    }
}
