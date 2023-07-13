using System.Text;
using Newtonsoft.Json;
using xpos319.viewmodels;

namespace xpos319.Services
{
    public class VariantService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration configuration;
        private string RouteApi = "";
        private VMResponse respon = new VMResponse();

        public VariantService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            this.RouteApi = this.configuration["RouteAPI"];
        }

        public async Task<List<VMTblVariant>> GetAllData()
        {
            List<VMTblVariant> data = new List<VMTblVariant>();
            string apiRespon = await client.GetStringAsync(RouteApi + "apiVariant/GetAllData");
            data = JsonConvert.DeserializeObject<List<VMTblVariant>>(apiRespon)!;

            return data;
        }

        public async Task<VMResponse> Create(VMTblVariant data)
        {
            //proses convert dari object ke string
            string json = JsonConvert.SerializeObject(data);

            //proses mengubah string menjadi json 
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //memanggil API
            var request = await client.PostAsync(RouteApi + "apiVariant/Save", content);

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

        public async Task<bool> CheckByName(string name, int id, int idCategory)
        {
            string apiRespon = await client.GetStringAsync(RouteApi + $"apiVariant/CheckByName/{name}/{id}/{idCategory}");
            bool isExist = JsonConvert.DeserializeObject<bool>(apiRespon);

            return isExist;
        }

        public async Task<VMTblVariant> GetDataById(int id)
        {
            VMTblVariant data = new VMTblVariant();
            string apiRespon = await client.GetStringAsync(RouteApi + $"apiVariant/GetDataById/{id}");
            data = JsonConvert.DeserializeObject<VMTblVariant>(apiRespon);

            return data;
        }

        public async Task<VMResponse> MultipleDelete(List<int> listId)
        {
            //proses convert dari object ke string
            string json = JsonConvert.SerializeObject(listId);

            //proses mengubah string menjadi json 
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //memanggil API
            var request = await client.PutAsync(RouteApi + "apiVariant/MultipleDelete", content);

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
