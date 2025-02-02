using projetIa.Service;
using projetIa.WebClient;

namespace projetIa.Tests
{
    public class Test_Segmentation
    {
        protected IService? service;
        protected IWebClient? webClient;

        public Test_Segmentation()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://149.7.5.30:21089/api/");
            this.webClient = new WebClient.WebClient(client);
            this.service = new Service.Service(webClient);
        }
    }
}