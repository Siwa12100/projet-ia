using System.IO.Compression;
using Microsoft.AspNetCore.Components.Forms;
using projetIa.WebClient;

namespace projetIa.Service
{
    public class Service : IService
    {
        private readonly IWebClient WebClient;

        public Service(IWebClient webClient)
        {
            WebClient = webClient;
        }

        public async Task<string?> ClassifierParGenre(IBrowserFile image)
        {
            var imageBytes = ConvertirIBrowserFileEnByteArray(image);
            if (imageBytes == null) return null;
            return await this.ClassifierParGenre(imageBytes);
        }

        public async Task<string?> ClassifierParGenre(byte[] image)
        {
            var resultatApi = await this.WebClient.ClassifierParGenreAsync(image);
            if (resultatApi == null) return null;
            return resultatApi.gender;
        }

        public Task<string?> ClassifierParPersonne(IBrowserFile image)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> ClassifierParPersonne(byte[] image)
        {
            var resultatApi = await this.WebClient.ClassifierParPersonneAsync(image);
            if (resultatApi == null) return null;
            return resultatApi.pesonne;
        }

        public async Task<List<string>?> SegmenterImage(IBrowserFile image)
        {
            var imageBytes = ConvertirIBrowserFileEnByteArray(image);
            if (imageBytes == null) return null;
            return await this.SegmenterImage(imageBytes);
        }

        public async Task<List<string>?> SegmenterImage(byte[] image)
        {
            var resultatApi = await this.WebClient.SegmenterImageAsync(image);
            if (resultatApi == null) return null;
            return ConvertirZipArchiveEnListeDeString(resultatApi);

        }

        protected byte[]? ConvertirIBrowserFileEnByteArray(IBrowserFile? image)
        {
            if (image == null) return null;

            byte[] imageBytes = new byte[image.Size];
            image.OpenReadStream().Read(imageBytes);

            return imageBytes;
        }

        protected List<string>? ConvertirZipArchiveEnListeDeString(ZipArchive? zipArchive)
        {
            if (zipArchive == null) return null;

            List<string> contenuFichiers = new List<string>();

            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                if (entry.Length > 0) // Éviter les répertoires vides
                {
                    using (StreamReader reader = new StreamReader(entry.Open()))
                    {
                        string contenu = reader.ReadToEnd();
                        contenuFichiers.Add(contenu);
                    }
                }
            }

            return contenuFichiers;
        }
    }
}


