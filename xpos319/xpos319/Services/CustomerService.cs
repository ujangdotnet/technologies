using System.Text;
using Newtonsoft.Json;
using xpos319.viewmodels;

namespace xpos319.Services
{
    public class CustomerService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration configuration;
        private string RouteApi = "";
        private VMResponse respon = new VMResponse();

        public CustomerService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            this.RouteApi = this.configuration["RouteAPI"];
        }

        public async Task<List<VMTblCustomer>> GetAllData()
        {
            List<VMTblCustomer> data = new List<VMTblCustomer>();
            string apiRespon = await client.GetStringAsync(RouteApi + "apiCustomer/GetAllData");
            data = JsonConvert.DeserializeObject<List<VMTblCustomer>>(apiRespon)!;

            return data;
        }

        public async Task<bool> CheckByEmail(string email, int id)
        {
            string apiRespon = await client.GetStringAsync(RouteApi + $"apiCustomer/CheckByEmail/{email}/{id}");
            bool isExist = JsonConvert.DeserializeObject<bool>(apiRespon);

            return isExist;
        }

        public async Task<VMResponse> Create(VMTblCustomer data)
        {
            //proses convert dari object ke string
            string json = JsonConvert.SerializeObject(data);

            //proses mengubah string menjadi json 
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //memanggil API
            var request = await client.PostAsync(RouteApi + "apiCustomer/Save", content);

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
