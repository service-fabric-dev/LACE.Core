namespace LACE.Core.Abstractions.Model
{
    public interface IMachines
    {
        IMachineAdapter this[string id]
        {
            get;
        }
    }
}
