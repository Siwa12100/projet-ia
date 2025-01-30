using projetIa.Service;
using projetIa.WebClient;

namespace projetIa.Tests;

public class Tests_lectures_fichiers
{

    protected IService? service;
    protected IWebClient? webClient;

    public Tests_lectures_fichiers()
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5000/api/");
        this.webClient = new WebClient.WebClient(client);
        this.service = new Service.Service(webClient);
    }
    

    [Fact]
    public void Test1()
    {
        Assert.Equal(4, 2 + 2);
    }
}
