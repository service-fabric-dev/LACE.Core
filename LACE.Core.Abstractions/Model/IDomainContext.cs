namespace LACE.Core.Abstractions.Model
{
    public interface IDomainContext
    {
        IMachines Machines { get; }
        IMeters Meters { get; }
        IMonitors Monitors { get; }

        //IFacts GetFacts();
        //IFact AddFact(IFact fact);
        //IFactReport Update(IFactReport report);
    }
}
