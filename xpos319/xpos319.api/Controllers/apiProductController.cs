using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class apiProductController : ControllerBase
    {
        private readonly XPOS_319Context db;
        private VMResponse respon = new VMResponse();
        private int IdUser = 1;

        public apiProductController(XPOS_319Context _db)
        {
            this.db = _db;
        }

        [HttpGet("GetAllData")]
        public List<VMTblProduct> GetAllData()
        {

            List<VMTblProduct> data = (from p in db.TblProducts
                                       join v in db.TblVariants on p.IdVariant equals v.Id
                                       join c in db.TblCategories on v.IdCategory equals c.Id
                                       where p.IsDelete == false
                                       select new VMTblProduct
                                       {
                                           Id = p.Id,
                                           NameProduct = p.NameProduct,
                                           Price = p.Price,
                                           Stock = p.Stock,
                                           Image = p.Image,

                                           IdVariant = p.IdVariant,
                                           NameVariant = v.NameVariant,

                                           IdCategory = v.IdCategory,
                                           NameCategory = c.NameCategory,

                                           CreateDate = p.CreateDate,

                                       }).ToList();
            return data;
        }

        [HttpGet("GetAllData_LeftJoin")]
        public List<VMTblProduct> GetAllData_LeftJoin()
        {

            List<VMTblProduct> data = (from p in db.TblProducts
                                       join v in db.TblVariants on p.IdVariant equals v.Id
                                       join c in db.TblCategories on v.IdCategory equals c.Id into tc from tcategory in tc.DefaultIfEmpty()
                                       where p.IsDelete == false && tcategory == null
                                       select new VMTblProduct
                                       {
                                           Id = p.Id,
                                           NameProduct = p.NameProduct,
                                           Price = p.Price,
                                           Stock = p.Stock,
                                           Image = p.Image,

                                           IdVariant = p.IdVariant,
                                           NameVariant = v.NameVariant,

                                           IdCategory = v.IdCategory,
                                           NameCategory = tcategory.NameCategory ?? "" //ini muncul ketika null (null diganti ke mtik)

                                       }).ToList();
            return data;
        }

        [HttpGet("GetDataById/{id}")]
        public VMTblProduct GetDataById(int id)
        {

            VMTblProduct data = (from p in db.TblProducts
                                       join v in db.TblVariants on p.IdVariant equals v.Id
                                       join c in db.TblCategories on v.IdCategory equals c.Id
                                       where p.IsDelete == false && p.Id == id
                                       select new VMTblProduct
                                       {
                                           Id = p.Id,
                                           NameProduct = p.NameProduct,
                                           Price = p.Price,
                                           Stock = p.Stock,
                                           Image = p.Image,

                                           IdVariant = p.IdVariant,
                                           NameVariant = v.NameVariant,

                                           IdCategory = v.IdCategory,
                                           NameCategory = c.NameCategory,

                                       }).FirstOrDefault()!;
            return data;
        }

        [HttpPost("Save")]
        public VMResponse Save(TblProduct data)
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
        public VMResponse Edit(TblProduct data)
        {
            TblProduct dt = db.TblProducts.Where(a => a.Id == data.Id).FirstOrDefault();

            if (dt != null)
            {
                dt.IdVariant = data.IdVariant;
                dt.NameProduct = data.NameProduct;
                dt.Price = data.Price;
                dt.Stock = data.Stock;
                if(data.Image != null)
                {
                    dt.Image = data.Image;
                }
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

        [HttpPut("Delete/{id}")]
        public VMResponse Delete(TblProduct data)
        {
            TblProduct dt = db.TblProducts.Where(a => a.Id == data.Id).FirstOrDefault();

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
        [HttpGet("CheckByName/{name}/{id}/{idVariant}")]
        public bool CheckIsExist(string name, int id, int idVariant)
        {
            TblProduct data = new TblProduct();

            if (id == 0)
            {
                data = db.TblProducts.Where(a => a.NameProduct == name && a.IsDelete == false && a.IdVariant == idVariant).FirstOrDefault()!;
            }
            else
            {
                data = db.TblProducts.Where(a => a.NameProduct == name && a.IsDelete == false && a.Id != id && a.IdVariant == idVariant).FirstOrDefault()!;
            }

            if (data != null)
            {
                return true;
            }

            return false;
        }
    }
}
