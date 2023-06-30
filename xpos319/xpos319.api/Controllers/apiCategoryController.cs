using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xpos319.datamodels;

namespace xpos319.api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class apiCategoryController : ControllerBase
	{
		private readonly XPOS_319Context db;

        public apiCategoryController(XPOS_319Context _db) //ini adalah sebuah constructor
        {
            this.db = _db;
        }

		[HttpGet("GetAllData")]
		public List<TblCategory> GetAllData(){

			List<TblCategory> data = db.TblCategories.Where(a => a.IsDelete == false).ToList();
			return data;
		}
    }
}
