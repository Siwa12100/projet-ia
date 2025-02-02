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
        public async Task Test_SegmentationImage()
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
        public async Task Test_ClassificationGenre()
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
    }
}