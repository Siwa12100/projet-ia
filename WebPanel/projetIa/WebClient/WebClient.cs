using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text.Json;
using projetIa.Dtos;

namespace projetIa.WebClient
{
    public class WebClient : IWebClient
    {
        private readonly HttpClient _httpClient;

        public WebClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ZipArchive?> SegmenterImageAsync(byte[] image)
        {
            using var content = new MultipartFormDataContent();
            using var imageContent = new ByteArrayContent(image);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // ou "image/png" selon ton image

            // 🔥 Ajout de l'image avec la clé "image"
            content.Add(imageContent, "image", "image.jpg");

            var response = await _httpClient.PostAsync("segmentation", content);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var stream = await response.Content.ReadAsStreamAsync();
            return new ZipArchive(stream);
        }

        public async Task<ResultatClassificationGenreDTO?> ClassifierParGenreAsync(byte[] image)
        {
            using var content = new MultipartFormDataContent();
            using var imageContent = new ByteArrayContent(image);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            content.Add(imageContent, "image", "image.jpg");

            var response = await _httpClient.PostAsync("gender-classify", content);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadFromJsonAsync<ResultatClassificationGenreDTO>();
            return json;
        }

        public async Task<ResultatClassificationPersonneDTO?> ClassifierParPersonneAsync(byte[] image)
        {
            using var content = new MultipartFormDataContent();
            using var imageContent = new ByteArrayContent(image);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            content.Add(imageContent, "image", "image.jpg");

            var response = await _httpClient.PostAsync("classification/personne", content);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadFromJsonAsync<ResultatClassificationPersonneDTO>();
            return json;
        }
    }
}
