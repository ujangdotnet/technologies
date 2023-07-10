using Newtonsoft.Json;
using System.Text;
using xpos319.viewmodels;

namespace xpos319.Services
{
    public class OrderService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration configuration;
        private string RouteAPI = "";
        private VMResponse respon = new VMResponse();

        public OrderService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            this.RouteAPI = this.configuration["RouteAPI"];
        }

        public async Task<VMResponse> SubmitOrder(VMOrderHeader data)
        {
            //proses convert dari object ke string
            string json = JsonConvert.SerializeObject(data);

            //proses mengubah string menjadi json 
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //memanggil API
            var request = await client.PostAsync(RouteAPI + "apiOrder/SubmitOrder", content);

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

        public async Task<List<VMOrderHeader>> ListHeaderDetails(int IdCustomer)
        {
            string apiResponse = await client.GetStringAsync(RouteAPI + $"apiOrder/GetDataOrderHeaderDetail/{IdCustomer}");

            List<VMOrderHeader> listHeader = JsonConvert.DeserializeObject<List<VMOrderHeader>>(apiResponse);
            return listHeader;
        }

        public async Task<int> CountTransaction(int IdCustomer)
        {
            string apiResponse = await client.GetStringAsync(RouteAPI + $"apiOrder/CountTransaction/{IdCustomer}");

            int count = JsonConvert.DeserializeObject<int>(apiResponse);
            return count;
        }
    }

    
}
