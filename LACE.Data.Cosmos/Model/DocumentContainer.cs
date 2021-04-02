using LACE.Data.Cosmos.Abstractions;
using LACE.Data.Cosmos.Enums;
using Newtonsoft.Json;

namespace LACE.Data.Cosmos.Model
{
    public class DocumentContainer<TDocument> where TDocument : class, IEtagged
    {
        public TDocument Value { get; set; }
        public string PartitionKey { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public DocumentState State { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag
        {
            get => Value?.ETag;
            set
            {
                if (Value != null)
                {
                    Value.ETag = value;
                }
            }
        }

        public static implicit operator TDocument(DocumentContainer<TDocument> wrapped) => wrapped.Value;
    }
}
