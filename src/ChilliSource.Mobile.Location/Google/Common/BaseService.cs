using System.Net.Http;
using System.Net.Http.Headers;

namespace ChilliSource.Mobile.Location.Google
{
    public abstract class BaseService
    {
        protected void AcceptJsonResponse(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
    }
}