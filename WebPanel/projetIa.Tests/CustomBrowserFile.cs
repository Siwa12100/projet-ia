namespace projetIa.Tests
{
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Threading.Tasks;

    public class CustomBrowserFile : IBrowserFile
    {
        public CustomBrowserFile(string filePath, string contentType, long size, string name)
        {
            Name = name;
            Size = size;
            LastModified = File.GetLastWriteTimeUtc(filePath);
            ContentType = contentType;
            FilePath = filePath;
        }

        public string Name { get; }

        public DateTimeOffset LastModified { get; }

        public long Size { get; }

        public string ContentType { get; }

        private string FilePath { get; }

        public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
        {
            if (Size > maxAllowedSize)
            {
                throw new IOException($"The file size exceeds the maximum allowed size of {maxAllowedSize} bytes.");
            }

            // Retourne le flux du fichier
            return new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public async Task<Stream> OpenReadStreamAsync(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
        {
            if (Size > maxAllowedSize)
            {
                throw new IOException($"The file size exceeds the maximum allowed size of {maxAllowedSize} bytes.");
            }

            // Retourne le flux du fichier
            return new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }

}