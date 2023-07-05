using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.viewmodels;


namespace Xpos319.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class apiVariantController : ControllerBase
    {
        private readonly XPOS_319Context db;
        private int idUser = 1;
        private VMResponse response = new VMResponse();

        public apiVariantController(XPOS_319Context _db)
        {
            this.db = _db;
        }

        [HttpGet("GetAllData")]
        public List<VMTblVariant> GetAllData()
        {
            List<VMTblVariant> data = (from v in db.TblVariants
                                       join c in db.TblCategories
                                       on v.IdCategory equals c.Id
                                       where v.IsDelete == false
                                       select new VMTblVariant
                                       {
                                           Id = v.Id,
                                           NameVariant = v.NameVariant,
                                           Description = v.Description,

                                           IdCategory = v.IdCategory,
                                           NameCategory = c.NameCategory,
                                       }).ToList();

            return data;
        }

        [HttpGet("GetDataById/{id}")]
        public VMTblVariant DataByID(int id)
        {
            VMTblVariant res = (from v in db.TblVariants
                                join c in db.TblCategories
                                on v.IdCategory equals c.Id
                                where v.IsDelete == false
                                select new VMTblVariant
                                {
                                    Id = v.Id,
                                    NameVariant = v.NameVariant,
                                    Description = v.Description,

                                    IdCategory = v.IdCategory,
                                    NameCategory = c.NameCategory,
                                }).FirstOrDefault()!;
            return res;
        }

        [HttpGet("GetDataByIdCategory/{id}")]
        public List<VMTblVariant> GetDataByIdCategory(int id)
        {
            List<VMTblVariant> data = (from v in db.TblVariants
                                       join c in db.TblCategories
                                       on v.IdCategory equals c.Id
                                       where v.IsDelete == false && v.IdCategory == id
                                       select new VMTblVariant
                                       {
                                           Id = v.Id,
                                           NameVariant = v.NameVariant,
                                           Description = v.Description,

                                           IdCategory = v.IdCategory,
                                           NameCategory = c.NameCategory,
                                       }).ToList();
            return data;
        }

        [HttpGet("CheckByName/{name}/{id}/{idCategory}")]
        public bool CheckIsExist(string name, int id, int idCategory)
        {
            TblVariant data = new TblVariant();

            if (id == 0)
            {
                data = db.TblVariants.Where(a => a.NameVariant == name && a.IsDelete == false && a.IdCategory == idCategory).FirstOrDefault()!;
            }
            else
            {
                data = db.TblVariants.Where(a => a.NameVariant == name && a.IsDelete == false && a.Id != id && a.IdCategory == idCategory).FirstOrDefault()!;
            }

            if (data != null)
            {
                return true;
            }

            return false;
        }

        [HttpPost("Save")]
        public VMResponse Save(TblVariant data)
        {
            data.Description = data.Description ?? "";
            data.CreateBy = idUser;
            data.CreateDate = DateTime.Now;
            data.IsDelete = false;

            try
            {
                db.Add(data);
                db.SaveChanges();

                response.Message = "Data successfully added";
            }
            catch (Exception e)
            {

                response.Success = false;
                response.Message = "Failed saved : " + e.Message;
            }

            return response;
        }

        [HttpPut("Edit")]

        public VMResponse Edit(TblVariant data)
        {
            TblVariant dt = db.TblVariants.Where(a => a.Id == data.Id).FirstOrDefault()!;

            if (dt != null)
            {
                dt.IdCategory = data.IdCategory;
                dt.NameVariant = data.NameVariant;
                dt.Description = data.Description ?? "";
                dt.UpdateBy = idUser;
                dt.UpdateDate = DateTime.Now; 

                try
                {
                    db.Update(dt);
                    db.SaveChanges();

                    response.Success = true;
                    response.Message = "Data successfully updated";
                }
                catch (Exception e)
                {

                    response.Success = false;
                    response.Message = "Failed update : " + e.Message;
                }

            }
            else
            {
                response.Success = false;
                response.Message = "Data not found.";
            }
            return response;
        }

        [HttpDelete("Delete/{id}")]

        public VMResponse Delete(int id)
        {
            TblVariant dt = db.TblVariants.Where(a => a.Id == id).FirstOrDefault()!;

            if (dt != null)
            {
                dt.IsDelete = true;
                dt.UpdateBy = idUser;
                dt.UpdateDate = DateTime.Now;

                try
                {
                    db.Update(dt);
                    db.SaveChanges();

                    response.Success = true;
                    response.Message = "Data successfully deleted";
                }
                catch (Exception e)
                {

                    response.Success = false;
                    response.Message = "Failed delete : " + e.Message;
                }

            }
            else
            {
                response.Success = false;
                response.Message = "Data not found.";
            }
            return response;
        }
    }
}
