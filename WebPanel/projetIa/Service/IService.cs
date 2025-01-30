using Microsoft.AspNetCore.Components.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace projetIa.Service
{
    
    public interface IService
    {
        Task<List<string>?> SegmenterImage(IBrowserFile image);
        Task<List<string>?> SegmenterImage(byte[] image);
        Task<string?> ClassifierParGenre(IBrowserFile image);
        Task<string?> ClassifierParGenre(byte[] image);
        Task<string?> ClassifierParPersonne(IBrowserFile image);
        Task<string?> ClassifierParPersonne(byte[] image);
    }
}