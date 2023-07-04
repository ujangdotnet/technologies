using System.Drawing;
using System.Text;
using Newtonsoft.Json;
using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.Services
{
	public class CategoryService
	{
		private static readonly HttpClient client = new HttpClient();
		private IConfiguration configuration;
        private string RouteApi = "";
        private VMResponse respon = new VMResponse();

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

        public async Task<VMResponse> Create(TblCategory dataParam)
        {
            //proses convert dari object ke string
            string json = JsonConvert.SerializeObject(dataParam);

            //proses mengubah string menjadi json 
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //memanggil API
            var request = await client.PostAsync(RouteApi + "apiCategory/Save", content);

            if (request.IsSuccessStatusCode)
            {
                //membaca respon dari API
                var apiRespon = await request.Content.ReadAsStringAsync();

                respon = JsonConvert.DeserializeObject<VMResponse>(apiRespon)!;
            }
            else
            {
                respon.Success = false;
                respon.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }

            return respon;
        }

        public async Task<bool> CheckCategoryByName(string nameCategory, int Id)
        {
            string apiRespon = await client.GetStringAsync(RouteApi + $"apiCategory/CheckCategoryByName/{nameCategory}/{Id}");
            bool isExist = JsonConvert.DeserializeObject<bool>(apiRespon);

            return isExist;
        }

        public async Task<TblCategory> GetDataById(int id)
        {
            TblCategory data = new TblCategory();
            string apiRespon = await client.GetStringAsync(RouteApi + $"apiCategory/GetDataById/{id}");
            data = JsonConvert.DeserializeObject<TblCategory>(apiRespon);

            return data;
        }

        public async Task<VMResponse> Edit(TblCategory dataParam)
        {
            //proses convert dari object ke string
            string json = JsonConvert.SerializeObject(dataParam);

            //proses mengubah string menjadi json 
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //memanggil API
            var request = await client.PutAsync(RouteApi + "apiCategory/Edit", content);

            if (request.IsSuccessStatusCode)
            {
                //membaca respon dari API
                var apiRespon = await request.Content.ReadAsStringAsync();

                respon = JsonConvert.DeserializeObject<VMResponse>(apiRespon)!;
            }
            else
            {
                respon.Success = false;
                respon.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }

            return respon;
        }

		public async Task<VMResponse> Delete(int id, int createBy)
		{

			//memanggil API
			var request = await client.DeleteAsync(RouteApi + $"apiCategory/Delete/{id}/{createBy}");

			if (request.IsSuccessStatusCode)
			{
				//membaca respon dari API
				var apiRespon = await request.Content.ReadAsStringAsync();

				respon = JsonConvert.DeserializeObject<VMResponse>(apiRespon)!;
			}
			else
			{
				respon.Success = false;
				respon.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
			}

			return respon;
		}
	}
}
