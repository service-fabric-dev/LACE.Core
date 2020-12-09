using Microsoft.Extensions.FileProviders;

namespace LACE.Core.Abstractions.Content
{
    public interface IEmbeddedContentLoader
    {
        TContent LoadJson<TContent>(string embeddedContentUri);
        IFileInfo GetFileInfo(string embeddedFileUri);
    }
}