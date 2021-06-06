using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PowerwallSniffer.Model;

namespace PowerwallSniffer
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public class PowerwallClient
    {
        private HttpClient HttpClient { get; }

        public PowerwallClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task Authenticate(string email, string password)
        {
            var request = new TokenRequest
            {
                Email = email,
                Password = password,
                Username = "customer",
                ForceSmOff = false
            };
            var payload = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8);
            var response = await HttpClient.PostAsync("/api/login/Basic", payload).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task<HttpResponseMessage> GetAggregates()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/meters/aggregates");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        
        public async Task<HttpResponseMessage> GetSiteSpecificMeterInformation()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/meters/site");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        
        public async Task<HttpResponseMessage> GetSolarInformation()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/meters/solar");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        
        public async Task<HttpResponseMessage> GetSystemStatusState()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/system_status/soe");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        
        public async Task<HttpResponseMessage> GetSiteMaster()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/sitemaster");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        
        public async Task<HttpResponseMessage> GetNumberOfPowerwalls()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/powerwalls");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        
        public async Task<HttpResponseMessage> GetRegistrationInformation()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/customer/registration");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        
        public async Task<HttpResponseMessage> GetGridStatus()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/system_status/gris_status");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        
        public async Task<HttpResponseMessage> GetUpdateStatus()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/system/update/status");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        
        public async Task<HttpResponseMessage> GetSiteInformation()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/site_info");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
        
        public async Task<HttpResponseMessage> GetSiteName()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/site_info/site_name");
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}
