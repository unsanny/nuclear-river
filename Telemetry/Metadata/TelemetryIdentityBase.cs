using NuClear.Model.Common;

namespace NuClear.Telemetry
{
    public abstract class TelemetryIdentityBase<T> : IdentityBase<T> 
        where T : IdentityBase<T>, new()
    {
        public virtual string Name
        {
            get { return GetType().Name.Replace("Identity", string.Empty); }
        }
    }
}