namespace LACE.Core.Abstractions.Model
{
    public interface IMeters
    {
        IMeterAdapter this[string id]
        {
            get;
        }
    }
}
