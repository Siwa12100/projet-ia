using Microsoft.AspNetCore.Components.Forms;
using projetIa.Service;
using projetIa.WebClient;

namespace projetIa.Tests
{
    public class Test_Segmentation
    {
        protected IService? service;
        protected IWebClient? webClient;
        protected readonly RecuperateurImages recuperateurImages;

        public Test_Segmentation()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://149.7.5.30:21089/api/");
            this.webClient = new WebClient.WebClient(client);
            this.service = new Service.Service(webClient);
            this.recuperateurImages = new RecuperateurImages();
        }

        [Fact]
        public async Task Test_SegmentationImage_WebClient()
        {
            var nomImageAEnvoyer = "image1.jpg";
            var image = recuperateurImages.RecupererImage(null, nomImageAEnvoyer);
            Assert.NotNull(image);

            var ImageByteArray = File.ReadAllBytes(image.FullName);
            Assert.NotNull(ImageByteArray);
            Assert.NotNull(webClient);
            var zipArchive = await webClient.SegmenterImageAsync(ImageByteArray);
            Assert.NotNull(zipArchive);
        }

        [Fact]
        public async Task Test_ClassificationGenreWebClient()
        {
            var nomImageAEnvoyer = "1.jpg";
            var nomDossierImage = "CropsManuels";
            var image = recuperateurImages.RecupererImage(nomDossierImage, nomImageAEnvoyer);
            Assert.NotNull(image);

            var ImageByteArray = File.ReadAllBytes(image.FullName);
            Assert.NotNull(ImageByteArray);
            Assert.NotNull(webClient);

            var resultat = await webClient.ClassifierParGenreAsync(ImageByteArray);
            Assert.NotNull(resultat);

            Assert.Equal("male", resultat.gender);
        }

        [Fact]
        public async Task Test_ClassificationGenre_Service_img_byte()
        {
            var nomImageAEnvoyer = "image1.jpg";
            var image = recuperateurImages.RecupererImage(null, nomImageAEnvoyer);
            Assert.NotNull(image);

            var ImageByteArray = File.ReadAllBytes(image.FullName);
            Assert.NotNull(ImageByteArray);
            Assert.NotNull(service);

            var resultat = await service.ClassifierParGenre(ImageByteArray);
            Assert.NotNull(resultat);

            Assert.Equal("male", resultat);
        }

        // [Fact]
        public async Task Test_Massifs_Hommes_Genre()
        {
            int tauxReussiteAPasser = 90;
            var nomDossierImage = "men";
            var fichiers = recuperateurImages.ListerFichiersDossier(nomDossierImage);
            Assert.NotEmpty(fichiers);

            int nbReussites = 0;
            int nbEchecs = 0;
            Assert.NotNull(service);

            foreach (var fichier in fichiers)
            {
                var ImageByteArray = File.ReadAllBytes(fichier.FullName);
                if (ImageByteArray == null)
                {
                    Console.WriteLine($"Impossible de lire le fichier {fichier.Name}");
                    continue;
                }

                var resultat = await service.ClassifierParGenre(ImageByteArray);
                if (resultat == null)
                {
                    Console.WriteLine($"Impossible de classifier le fichier {fichier.Name}");
                    continue;
                }

                if (resultat == "male")
                {
                    nbReussites++;
                }
                else
                {
                    nbEchecs++;
                }

                Console.WriteLine($"Résultat pour {fichier.Name} : {resultat}");
            }

            Console.WriteLine($"Nombre de réussites : {nbReussites}");
            Console.WriteLine($"Nombre d'échecs : {nbEchecs}");
            Console.WriteLine($"Pourcentage de réussite : {100 * nbReussites / (nbReussites + nbEchecs)}%");

            int tauxReussiteReel = 100 * nbReussites / (nbReussites + nbEchecs);
            Assert.True(tauxReussiteReel >= tauxReussiteAPasser);

            // Nombre de réussites : 786
            // Nombre d'échecs : 33
            // Pourcentage de réussite : 95%
        }

        // [Fact]
        public async Task Test_Massifs_Femmes_Genre()
        {
            int tauxReussiteAPasser = 80;
            var nomDossierImage = "women";

            var fichiers = recuperateurImages.ListerFichiersDossier(nomDossierImage);
            Assert.NotEmpty(fichiers);

            int nbReussites = 0;
            int nbEchecs = 0;
            Assert.NotNull(service);

            foreach (var fichier in fichiers)
            {
                var ImageByteArray = File.ReadAllBytes(fichier.FullName);
                if (ImageByteArray == null)
                {
                    Console.WriteLine($"Impossible de lire le fichier {fichier.Name}");
                    continue;
                }

                var resultat = await service.ClassifierParGenre(ImageByteArray);
                if (resultat == null)
                {
                    Console.WriteLine($"Impossible de classifier le fichier {fichier.Name}");
                    continue;
                }

                if (resultat == "female")
                {
                    nbReussites++;
                }
                else
                {
                    nbEchecs++;
                }

                Console.WriteLine($"Résultat pour {fichier.Name} : {resultat}");
            }

            Console.WriteLine($"Nombre de réussites : {nbReussites}");
            Console.WriteLine($"Nombre d'échecs : {nbEchecs}");
            Console.WriteLine($"Pourcentage de réussite : {100 * nbReussites / (nbReussites + nbEchecs)}%");

            int tauxReussiteReel = 100 * nbReussites / (nbReussites + nbEchecs);
            Assert.True(tauxReussiteReel>= tauxReussiteAPasser);

            // Nombre de réussites : 720
            // Nombre d'échecs : 113
            // Pourcentage de réussite : 86%
        }
    }
}