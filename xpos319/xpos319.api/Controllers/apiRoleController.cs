using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xpos319.api.Service;
using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class apiRoleController : ControllerBase
    {
        //this start
        private readonly XPOS_319Context db;
        VMResponse respon = new VMResponse();
        private RolesServices rolesService;
        private int IdUser = 1;

        public apiRoleController(XPOS_319Context _db)
        {
            this.db = _db;
            this.rolesService = new RolesServices(db);
        }
        //this.end


        [HttpGet("GetAllData")]
        public List<TblRole> GetAllData()
        {

            List<TblRole> data = db.TblRoles.Where(a => a.IsDelete == false).ToList();
            return data;
        }

        [HttpGet("CheckRoleByName/{name}/{id}")]
        public bool CheckRoleByName(string name, int id)
        {
            TblRole data = new TblRole();

            if (id == 0)
            {
                data = db.TblRoles.Where(a => a.RoleName == name && a.IsDelete == false).FirstOrDefault();
            }
            else
            {
                data = db.TblRoles.Where(a => a.RoleName == name && a.IsDelete == false && a.Id != id).FirstOrDefault();
            }

            if (data != null)
            {
                return true;
            }

            return false;
        }

        [HttpPost("Save")]
        public VMResponse Save(TblRole data)
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

        [HttpGet("GetDataById/{id}")]
        public async VMTblRole DataById(int id)
        {
            //TblRole result = db.TblRoles.Where(a => a.Id == id).FirstOrDefault();
            VMTblRole result = db.TblRoles.Where(a => a.Id == id).Select(a => new VMTblRole()
                                                                        {
                                                                          Id = a.Id,
                                                                          RoleName = a.RoleName,
                                                                        }).FirstOrDefault()!;

            result.role_menu = await rolesService.GetMenuAccessParentChildByRoleID(result.Id, );
            return result;
        }

        [HttpPut("Edit")]
        public VMResponse Edit(TblRole data)
        {
            TblRole dt = db.TblRoles.Where(a => a.Id == data.Id).FirstOrDefault();

            if (dt != null)
            {
                dt.RoleName = data.RoleName;
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

        [HttpDelete("Delete/{id}/{createBy}")]

        public VMResponse Delete(int id, int createBy)
        {
            TblRole dt = db.TblRoles.Where(a => a.Id == id).FirstOrDefault();

            if (dt != null)
            {
                dt.IsDelete = true;
                dt.UpdateBy = createBy;
                dt.UpdateDate = DateTime.Now;

                try
                {
                    db.Update(dt);
                    db.SaveChanges();

                    respon.Message = $"data {dt.RoleName} success deleted";
                }
                catch (Exception e)
                {
                    respon.Success = false;
                    respon.Message = "data failed to delete" + e.Message;
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
