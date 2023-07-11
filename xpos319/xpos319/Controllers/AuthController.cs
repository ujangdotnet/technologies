using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
    public class AuthController : Controller
    {
        private AuthService authService;
        private RoleService roleService;
        VMResponse respon = new VMResponse();

        public AuthController(AuthService _authService, RoleService _roleService)
        {
            this.authService = _authService;
            this.roleService = _roleService;
        }
        public IActionResult Login()
        {
            return PartialView();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<JsonResult> LoginSubmit(string email, string password)
        {
            VMTblCustomer customer = await authService.CheckLogin(email, password);

            if(customer != null)
            {
                respon.Message = $"Hello, {customer.NameCustomer} welcome to XPOS";
                HttpContext.Session.SetString("NameCustomer", customer.NameCustomer);
                HttpContext.Session.SetInt32("IdCustomer", customer.Id);
                HttpContext.Session.SetInt32("IdRole", customer.IdRole ?? 0);

            }
            else
            {
                respon.Message = $"Ooops, {email} not found, please check your mail";
            }

            return Json(new {dataRespon = respon});
        }

        public async Task<IActionResult> Register()
        {
            VMTblCustomer data = new VMTblCustomer();
            List<TblRole> roleList = await roleService.GetAllData();
            ViewBag.RoleList = roleList;

            return View(data);
        }
    }
}
