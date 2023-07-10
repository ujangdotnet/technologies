using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xpos319.Services;
using xpos319.viewmodels;

namespace xpos319.Controllers
{
    public class OrderController : Controller
    {
        private readonly ProductService productService;
        private readonly OrderService orderService;
        private int IdUser = 1;

        public OrderController(ProductService _productService, OrderService _orderService)
        {
            this.productService = _productService;
            this.orderService = _orderService;
        }

        public async Task<IActionResult> Catalog(VMSearchPage dataSearch)
        {
            List<VMTblProduct> dataProduct = await productService.GetAllData();
            dataSearch.MinAmount = dataSearch.MinAmount == null ? decimal.MinValue : dataSearch.MinAmount;
            dataSearch.MaxAmount = dataSearch.MaxAmount == null ? decimal.MaxValue : dataSearch.MaxAmount;

            if(dataSearch.NameProduct != null)
            {
                //dataProduct = dataProduct.Where( a => a.NameProduct == dataSearch.NameProduct ).ToList();
                dataProduct = dataProduct.Where( a => a.NameProduct.ToLower().Contains(dataSearch.NameProduct.ToLower())).ToList();
            }

            if(dataSearch.MinAmount != null && dataSearch.MaxAmount != null)
            {
                dataProduct = dataProduct.Where(a => a.Price >= dataSearch.MinAmount && a.Price <= dataSearch.MaxAmount).ToList();
            }

            //get session in vmorder header
            VMOrderHeader dataHeader = HttpContext.Session.GetComplexData<VMOrderHeader>("ListCart");
            if(dataHeader == null)
            {
                dataHeader = new VMOrderHeader();
                dataHeader.ListDetails = new List<VMOrderDetail>();
            }

            var ListDetail = JsonConvert.SerializeObject(dataHeader.ListDetails);
            ViewBag.dataHeader = dataHeader;
            ViewBag.dataDetail = ListDetail;

            ViewBag.Search = dataSearch;
            ViewBag.CurrentPageSize = dataSearch.CurrentPageSize;

            return View(PagInatedList<VMTblProduct>.CreateAsync(dataProduct, dataSearch.PageNumber ?? 1, dataSearch.PageSize ?? 3));
        }

        [HttpPost]
        public JsonResult SetSession(VMOrderHeader dataHeader)
        {
            HttpContext.Session.SetComplexData("ListCart", dataHeader);

            return Json("");
        }

        public JsonResult RemoveSession()
        {
            HttpContext.Session.Remove("ListCart");

            return Json("");
        }

        public IActionResult OrderPreview(VMOrderHeader dataHeader)
        {
            return PartialView(dataHeader);
        }

        public IActionResult SearchMenu()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> SubmitOrder(VMOrderHeader dataHeader)
        {
            dataHeader.IdCustomer = IdUser;

            VMResponse respon = await orderService.SubmitOrder(dataHeader);
            return Json(respon);
        }

        public async Task<IActionResult> HistoryOrder(VMSearchPage dataSearch)
        {
            //int idCustomer = HttpContext.Session.GetInt32("IdCustomer") ?? 0;
            int IdCustomer = IdUser;
            List<VMOrderHeader> data = await orderService.ListHeaderDetails(IdCustomer);
            int count = await orderService.CountTransaction(IdCustomer);
            ViewBag.Count = count;

            dataSearch.MinDate = dataSearch.MinDate == null ? DateTime.MinValue : dataSearch.MinDate;
            dataSearch.MaxDate = dataSearch.MaxDate == null ? DateTime.MaxValue : dataSearch.MaxDate;

            dataSearch.MinAmount = dataSearch.MinAmount == null ? decimal.MinValue : dataSearch.MinAmount;
            dataSearch.MaxAmount = dataSearch.MaxAmount == null ? decimal.MaxValue : dataSearch.MaxAmount;

            if (!string.IsNullOrEmpty(dataSearch.codeTransaction))
            {
                data = data.Where(a => a.CodeTransaction.ToLower().Contains(dataSearch.codeTransaction.ToLower())).ToList();
            }

            if (dataSearch.MinDate != null && dataSearch.MaxDate != null)
            {
                data = data.Where(a => a.CreateDate >= dataSearch.MinDate && a.CreateDate <= dataSearch.MaxDate).ToList();
            }

            if (dataSearch.MinAmount != null && dataSearch.MaxAmount != null)
            {
                data = data.Where(a => a.Amount >= dataSearch.MinAmount && a.Amount <= dataSearch.MaxAmount).ToList();
            }

            ViewBag.Search = dataSearch;

            return View(data);
        }

        public IActionResult Search()
        {
            return PartialView();
        }

    }
}
