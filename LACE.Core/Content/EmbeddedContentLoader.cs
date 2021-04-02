using LACE.Core.Abstractions.Content;
using LACE.Core.Extensions;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LACE.Core.Content
{
    public class EmbeddedContentLoader : IEmbeddedContentLoader
    {
        private readonly Assembly _assembly;

        public EmbeddedContentLoader(Assembly assembly)
        {
            _assembly = assembly.Guard(nameof(assembly));
        }

        public string LoadString(string embeddedContentUri)
        {
            if (embeddedContentUri.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }
            
            var resource = _assembly
                .GetManifestResourceNames()
                .FirstOrDefault(file => file.EndsWith(embeddedContentUri));
            if (resource.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            var stream = _assembly.GetManifestResourceStream(resource);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public TContent LoadJson<TContent>(string embeddedContentUri)
        {
            var content = LoadString(embeddedContentUri);
            return JsonConvert.DeserializeObject<TContent>(content);
        }
    }
}
