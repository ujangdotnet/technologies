using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiCustomerController : ControllerBase
    {
        private readonly XPOS_319Context db;
        private VMResponse respon = new VMResponse();

        public ApiCustomerController(XPOS_319Context _db)
        {
            this.db = _db;
        }

        [HttpGet("GetAllData")]
        public List<VMTblCustomer> GetAllData()
        {
            List<VMTblCustomer> data = (from c in db.TblCustomers
                                       join r in db.TblRoles on c.IdRole equals r.Id
                                       where c.IsDelete == false
                                       select new VMTblCustomer
                                       {
                                           Id = c.Id,
                                           NameCustomer = c.NameCustomer,
                                           Email = c.Email,
                                           Password = c.Password,
                                           Address = c.Address,
                                           Phone = c.Phone,

                                           IdRole = c.IdRole,
                                           NameRole = r.RoleName,

                                       }).ToList();
            return data;
        }

        [HttpGet("GetDataById/{id}")]
        public VMTblCustomer GetDataById(int id)
        {

            VMTblCustomer data = (from c in db.TblCustomers
                                  join r in db.TblRoles on c.IdRole equals r.Id
                                  where c.IsDelete == false && c.Id == id
                                  select new VMTblCustomer
                                  {
                                      Id = c.Id,
                                      NameCustomer = c.NameCustomer,
                                      Email = c.Email,
                                      Password = c.Password,
                                      Address = c.Address,
                                      Phone = c.Phone,

                                      IdRole = c.IdRole,
                                      NameRole = r.RoleName,

                                  }).FirstOrDefault()!;
            return data;
        }
    }
}
