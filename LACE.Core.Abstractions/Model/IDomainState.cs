namespace LACE.Core.Abstractions.Model
{
    public interface IDomainState
    {
        IFacts GetFacts();
        IFact AddFact(IFact fact);
        IFactReport Update(IFactReport report);
    }
}
