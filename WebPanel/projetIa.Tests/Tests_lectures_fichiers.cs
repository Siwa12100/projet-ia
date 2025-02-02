using projetIa.Service;
using projetIa.WebClient;

namespace projetIa.Tests;

public class Tests_lectures_fichiers
{

    protected IService? service;
    protected IWebClient? webClient;

    private readonly RecuperateurImages recuperateurImages;

    public Tests_lectures_fichiers()
{
    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:5000/api/");
    this.webClient = new WebClient.WebClient(client);
    this.service = new Service.Service(webClient);
    this.recuperateurImages = new RecuperateurImages();

}

    

    [Fact]
    public void Test1()
    {
        Assert.Equal(4, 2 + 2);
    }

    [Fact]
    public void Test_recuperation_fichier_racine()
    {
        // Tester la récupération de l'image à la racine
        var fichier = recuperateurImages.RecupererImage(null, "image1.jpg");
        Assert.NotNull(fichier);
        Assert.Equal("image1.jpg", fichier.Name);
        Console.WriteLine($"Fichier récupéré : {fichier.FullName}");
    }

    [Fact]
    public void Test_lister_fichiers_dossier()
    {
        // Tester la liste des fichiers dans TestRecuperation
        var fichiers = recuperateurImages.ListerFichiersDossier("TestRecuperation");
        Assert.NotEmpty(fichiers);

        // Vérifier les noms des fichiers
        var nomsAttendus = new[] { "agora2.png", "beacon.png", "conseilAlliance1.png", "endportalbloc.webp", "icone.png" };
        foreach (var nomAttendu in nomsAttendus)
        {
            Assert.Contains(fichiers, f => f.Name == nomAttendu);
        }

        Console.WriteLine("Fichiers récupérés dans TestRecuperation :");
        foreach (var fichier in fichiers)
        {
            Console.WriteLine(fichier.FullName);
        }
    }

}
