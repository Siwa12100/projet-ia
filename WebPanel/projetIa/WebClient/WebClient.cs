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
            var content = new ByteArrayContent(image);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
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
            var content = new ByteArrayContent(image);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var response = await _httpClient.PostAsync("classification/genre", content);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadFromJsonAsync<ResultatClassificationGenreDTO>();
            return json;
        }

        public async Task<ResultatClassificationPersonneDTO?> ClassifierParPersonneAsync(byte[] image)
        {
            var content = new ByteArrayContent(image);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
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