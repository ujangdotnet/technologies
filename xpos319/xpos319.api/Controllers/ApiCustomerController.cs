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
        private int IdUser = 1;

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
                                           RoleName = r.RoleName,

                                           IsDelete = c.IsDelete,
                                           CreateDate = c.CreateDate,

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
                                      RoleName = r.RoleName,

                                      IsDelete = c.IsDelete,
                                      CreateDate = c.CreateDate,

                                  }).FirstOrDefault()!;
            return data;
        }

        [HttpGet("CheckByEmail/{email}/{id}")]
        public bool CheckByEmail(string email, int id)
        {
            TblCustomer data = new TblCustomer();

            if (id == 0)
            {
                data = db.TblCustomers.Where(a => a.Email == email && a.IsDelete == false).FirstOrDefault()!;
            }
            else
            {
                data = db.TblCustomers.Where(a => a.Email == email && a.IsDelete == false && a.Id != id).FirstOrDefault()!;
            }

            if (data != null)
            {
                return true;
            }

            return false;
        }

        [HttpPost("Save")]
        public VMResponse Save(TblCustomer data)
        {
            data.CreateBy = IdUser;
            data.CreateDate = DateTime.Now;
            data.IsDelete = false;

            try
            {
                db.Add(data);
                db.SaveChanges();

                respon.Message = "Data success saved";
            }
            catch (Exception e)
            {
                respon.Success = false;
                respon.Message = e.Message;
            }

            return respon;
        }

        [HttpPut("Edit")]
        public VMResponse Edit(TblCustomer data)
        {
            TblCustomer dt = db.TblCustomers.Where(a => a.Id == data.Id).FirstOrDefault();

            if (dt != null)
            {
                dt.NameCustomer = data.NameCustomer;
                dt.Email = data.Email;
                dt.Password = data.Password;
                dt.Address = data.Address;
                dt.Phone = data.Phone;
                dt.IdRole = data.IdRole;
                dt.UpdateBy = IdUser;
                dt.UpdateDate = DateTime.Now;

                try
                {
                    db.Update(dt);
                    db.SaveChanges();

                    respon.Message = "Data success edited";
                }
                catch (Exception e)
                {
                    respon.Success = false;
                    respon.Message = e.Message;
                }
            }
            else
            {
                respon.Success = false;
                respon.Message = "data not found";
            }

            return respon;
        }

        [HttpDelete("Delete/{id}")]
        public VMResponse Delete(int id)
        {
            TblCustomer dt = db.TblCustomers.Where(a => a.Id == id).FirstOrDefault();

            if (dt != null)
            {
                dt.IsDelete = true;
                dt.UpdateBy = IdUser;
                dt.UpdateDate = DateTime.Now;

                try
                {
                    db.Update(dt);
                    db.SaveChanges();

                    respon.Message = "Data success deleted";
                }
                catch (Exception e)
                {
                    respon.Success = false;
                    respon.Message = e.Message;
                }
            }
            else
            {
                respon.Success = false;
                respon.Message = "data not found";
            }

            return respon;
        }
    }
}
