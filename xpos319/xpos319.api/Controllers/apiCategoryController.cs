using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class apiCategoryController : ControllerBase
	{
		//this.start
		private readonly XPOS_319Context db;
		VMResponse respon = new VMResponse();
		private int IdUser = 1;
        public apiCategoryController(XPOS_319Context _db) //ini adalah sebuah constructor
        {
            this.db = _db;
        } 
		//this.end

		[HttpGet("GetAllData")]
		public List<TblCategory> GetAllData(){

			List<TblCategory> data = db.TblCategories.Where(a => a.IsDelete == false).ToList();
			return data;
		}

		[HttpGet("GetDataById/{id}")]
		public TblCategory DataById(int id)
		{
			TblCategory result = db.TblCategories.Where(a => a.Id == id).FirstOrDefault();

			return result;
		}

		[HttpGet("CheckCategoryByName/{name}/{id}")]
		public bool CheckName(string name, int id)
		{
			TblCategory data = new TblCategory();

			if (id == 0)
			{
				data = db.TblCategories.Where(a => a.NameCategory == name && a.IsDelete == false).FirstOrDefault();
			}
			else
			{
				data = db.TblCategories.Where(a => a.NameCategory == name && a.IsDelete == false && a.Id != id).FirstOrDefault();
			}

			if (data != null)
			{
				return true;
			}

			return false;
		}

        [HttpPost("Save")]
		public VMResponse Save(TblCategory data)
		{
			data.Description = data.Description ?? "";
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
		public VMResponse Edit(TblCategory data)
		{
			TblCategory dt = db.TblCategories.Where(a => a.Id == data.Id).FirstOrDefault();

			if(dt != null)
			{
				dt.NameCategory = data.NameCategory;
				dt.Description = data.Description ?? "";
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
			TblCategory dt = db.TblCategories.Where(a => a.Id == id).FirstOrDefault();

			if(dt != null)
			{
				dt.IsDelete = true;
				dt.UpdateBy = createBy;
				dt.UpdateDate = DateTime.Now;

				try
				{
					db.Update(dt);
					db.SaveChanges();

					respon.Message = $"data {dt.NameCategory} success deleted";
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
