using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using LACE.Core.Abstractions.Content;
using LACE.Core.Extensions;
using Newtonsoft.Json;

namespace LACE.Core.Content
{
    public class EmbeddedContentLoader : IEmbeddedContentLoader
    {
        private static readonly EmbeddedFileProvider Provider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        
        public TContent LoadJson<TContent>(string embeddedContentUri)
        {
            if (embeddedContentUri.IsNullOrWhiteSpace())
            {
                return default;
            }

            var stream = Provider.GetFileInfo(embeddedContentUri).CreateReadStream();
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<TContent>(content);
            }
        }

        public IFileInfo GetFileInfo(string embeddedFileUri)
        {
            return embeddedFileUri.IsNullOrWhiteSpace() ? default : Provider.GetFileInfo(embeddedFileUri);
        }
    }
}
