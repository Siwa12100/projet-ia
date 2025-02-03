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
            this._httpClient.BaseAddress = new Uri("http://149.7.5.30:21089/api/");
        }

        public async Task<ZipArchive?> SegmenterImageAsync(byte[] image)
        {
            using var content = new MultipartFormDataContent();
            using var imageContent = new ByteArrayContent(image);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

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

            var response = await _httpClient.PostAsync("iut-detection", content);
            if (!response.IsSuccessStatusCode)
            {
                string contenuReponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Contenu reponse : " + contenuReponse);
                return null;
            }

            var json = await response.Content.ReadFromJsonAsync<ResultatClassificationPersonneDTO>();
            return json;
        }
    }
}
