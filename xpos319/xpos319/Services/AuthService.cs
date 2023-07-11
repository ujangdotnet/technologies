using Newtonsoft.Json;
using xpos319.viewmodels;

namespace xpos319.Services
{
    public class AuthService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration configuration;
        private string RouteApi = "";
        private VMResponse respon = new VMResponse();

        public AuthService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
            this.RouteApi = this.configuration["RouteAPI"];
        }

        public async Task<VMTblCustomer> CheckLogin(string email, string password)
        {
            VMTblCustomer data = new VMTblCustomer();
            string apiRespon = await client.GetStringAsync(RouteApi + $"apiAuth/CheckLogin/{email}/{password}");
            data = JsonConvert.DeserializeObject<VMTblCustomer>(apiRespon)!;

            return data;
        }

        
    }
}
