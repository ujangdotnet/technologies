using System.Text;
using Newtonsoft.Json;
using xpos319.datamodels;
using xpos319.viewmodels;

namespace xpos319.Services
{
    public class RoleService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration configuration;
        private string RouteApi = "";
        private VMResponse respon = new VMResponse();

        public RoleService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            this.RouteApi = this.configuration["RouteAPI"];
        }

        public async Task<List<TblRole>> GetAllData()
        {
            List<TblRole> data = new List<TblRole>();
            string apiRespon = await client.GetStringAsync(RouteApi + "apiRole/GetAllData");
            data = JsonConvert.DeserializeObject<List<TblRole>>(apiRespon);
            return data;
        }

        public async Task<VMResponse> Create(TblRole dataParam)
        {
            //proses convert dari object ke string
            string json = JsonConvert.SerializeObject(dataParam);

            //proses mengubah string menjadi json 
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //memanggil API
            var request = await client.PostAsync(RouteApi + "apiRole/Save", content);

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

        public async Task<bool> CheckRoleByName(string name, int Id)
        {
            string apiRespon = await client.GetStringAsync(RouteApi + $"apiRole/CheckRoleByName/{name}/{Id}");
            bool isExist = JsonConvert.DeserializeObject<bool>(apiRespon);

            return isExist;
        }

        public async Task<VMTblRole> GetDataById(int id)
        {
            VMTblRole data = new VMTblRole();
            string apiRespon = await client.GetStringAsync(RouteApi + $"apiRole/GetDataById/{id}");
            data = JsonConvert.DeserializeObject<VMTblRole>(apiRespon);

            return data;
        }

        public async Task<VMResponse> Edit(TblRole dataParam)
        {
            //proses convert dari object ke string
            string json = JsonConvert.SerializeObject(dataParam);

            //proses mengubah string menjadi json 
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //memanggil API
            var request = await client.PutAsync(RouteApi + "apiRole/Edit", content);

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
            var request = await client.DeleteAsync(RouteApi + $"apiRole/Delete/{id}/{createBy}");

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
