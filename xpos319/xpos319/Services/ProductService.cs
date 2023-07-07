using System.Text;
using Newtonsoft.Json;
using xpos319.viewmodels;

namespace xpos319.Services
{
    public class ProductService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration configuration;
        private string RouteApi = "";
        private VMResponse respon = new VMResponse();

        public ProductService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            this.RouteApi = this.configuration["RouteAPI"];
        }

        public async Task<List<VMTblProduct>> GetAllData()
        {
            List<VMTblProduct> data = new List<VMTblProduct>();
            string apiRespon = await client.GetStringAsync(RouteApi + "apiProduct/GetAllData");
            data = JsonConvert.DeserializeObject<List<VMTblProduct>>(apiRespon)!;

            return data;
        }

        public async Task<VMResponse> Create(VMTblProduct data)
        {
            //proses convert dari object ke string
            string json = JsonConvert.SerializeObject(data);

            //proses mengubah string menjadi json 
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //memanggil API
            var request = await client.PostAsync(RouteApi + "apiProduct/Save", content);

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

        public async Task<bool> CheckByName(string name, int id, int idVariant)
        {
            string apiRespon = await client.GetStringAsync(RouteApi + $"apiProduct/CheckByName/{name}/{id}/{idVariant}");
            bool isExist = JsonConvert.DeserializeObject<bool>(apiRespon);

            return isExist;
        }

        public async Task<VMTblProduct> GetDataById(int id)
        {
            VMTblProduct data = new VMTblProduct();
            string apiRespon = await client.GetStringAsync(RouteApi + $"apiProduct/GetDataById/{id}");
            data = JsonConvert.DeserializeObject<VMTblProduct>(apiRespon);

            return data;
        }

        public async Task<VMResponse> Edit(VMTblProduct data)
        {
            //proses convert dari object ke string
            string json = JsonConvert.SerializeObject(data);

            //proses mengubah string menjadi json 
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //memanggil API
            var request = await client.PutAsync(RouteApi + "apiProduct/Edit", content);

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

        public async Task<VMResponse> Delete(int id)
        {

            //memanggil API
            var request = await client.DeleteAsync(RouteApi + $"apiProduct/Delete/{id}");

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
