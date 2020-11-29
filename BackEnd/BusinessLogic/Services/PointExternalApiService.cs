using BusinessLogic.Models.Data;

using RestSharp;

using System.Net;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class PointExternalApiService
    {
        public async Task<bool> CheckConditionByApi(string url, ActionParseParameters parameters)
        {
            RestClient client = new RestClient(url);

            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddJsonBody(parameters);

            IRestResponse<bool> response = await client.ExecuteAsync<bool>(request);

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            return response.Data;
        }

        public async Task NotifyAction(string url)
        {
            RestClient client = new RestClient(url);

            RestRequest request = new RestRequest();
            request.Method = Method.POST;

            IRestResponse<bool> response = await client.ExecuteAsync<bool>(request);
        }
    }
}
