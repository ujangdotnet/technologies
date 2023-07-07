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
    }
}
