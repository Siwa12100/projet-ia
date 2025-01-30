using System.IO.Compression;
using projetIa.Dtos;

namespace projetIa.WebClient
{
    public interface IWebClient
    {
        Task<ZipArchive?> SegmenterImageAsync(byte[] image);
        Task<ResultatClassificationGenreDTO?> ClassifierParGenreAsync(byte[] image);
        Task<ResultatClassificationPersonneDTO?> ClassifierParPersonneAsync(byte[] image);
    }
}