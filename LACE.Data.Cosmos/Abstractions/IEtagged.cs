namespace LACE.Data.Cosmos.Abstractions
{
    public interface IEtagged
    {
        public string ETag { get; set; }
    }
}
