using Newtonsoft.Json;

namespace LACE.Data.Cosmos.Model
{
    public class DocumentContainer<TDocument>
    {
        public TDocument Value { get; set; }

        public string PartitionKey { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; set; }

        public DocumentState State { get; set; }

        public static implicit operator TDocument(DocumentContainer<TDocument> wrapped) => wrapped.Value;
    }
}
