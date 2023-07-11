using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class apiAuthController : ControllerBase
    {
        private readonly XPOS_319Context db;
        private int idUser = 1;
        private VMResponse response = new VMResponse();

        public apiAuthController(XPOS_319Context _db)
        {
            this.db = _db;
        }

        [HttpGet("CheckLogin/{email}/{password}")]
        public VMTblCustomer CheckLogin(string email, string password)
        {
            VMTblCustomer data = (from c in db.TblCustomers
                                  join r in db.TblRoles on c.IdRole equals r.Id
                                  where c.IsDelete == false && c.Email == email && c.Password == password
                                  select new VMTblCustomer
                                  {
                                      Id = c.Id,
                                      NameCustomer = c.NameCustomer,
                                   
                                      IdRole = c.IdRole,
                                      RoleName = r.RoleName,

                                  }).FirstOrDefault()!;
            return data;
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
                                            RoleName = r.RoleName,

                                            IsDelete = c.IsDelete,
                                            CreateDate = c.CreateDate,

                                        }).ToList();
            return data;
        }
    }
}
