using LACE.Data.Cosmos.Abstractions;

namespace LACE.Data.Cosmos
{
    public interface IPartitionFactory
    {
        IDocumentRepository<TType> Build<TType>(string name);
    }
}