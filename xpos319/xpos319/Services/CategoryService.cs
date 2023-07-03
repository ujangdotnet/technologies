using Newtonsoft.Json;
using xpos319.datamodels;

namespace xpos319.Services
{
	public class CategoryService
	{
		private static readonly HttpClient client = new HttpClient();
		private IConfiguration configuration;
        private string RouteApi = "";

        public CategoryService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            this.RouteApi = this.configuration["RouteAPI"];
        }

        public async Task<List<TblCategory>> GetAllData()
        {
            List<TblCategory> data = new List<TblCategory>();
            string apiRespon = await client.GetStringAsync(RouteApi + "apiCategory/GetAllData");
            data = JsonConvert.DeserializeObject<List<TblCategory>>(apiRespon);

            return data;
        }
    }
}
