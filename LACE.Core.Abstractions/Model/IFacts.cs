namespace LACE.Core.Abstractions.Model
{
    public interface IFacts
    {
        bool ContainsKey(string factKey);
        IFact Get(string factKey);
    }
}
