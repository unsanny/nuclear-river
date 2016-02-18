using NuClear.CustomerIntelligence.Storage.Identitites.Connections;
using NuClear.Storage.API.ConnectionStrings;

namespace NuClear.CustomerIntelligence.StateInitialization
{
    internal sealed class ConnectionString
    {
        public static IConnectionStringIdentity Erm =
            ErmConnectionStringIdentity.Instance;

        public static IConnectionStringIdentity Facts =
            FactsConnectionStringIdentity.Instance;

        public static IConnectionStringIdentity Bit =
            BitConnectionStringIdentity.Instance;

        public static IConnectionStringIdentity CustomerIntelligence =
            CustomerIntelligenceConnectionStringIdentity.Instance;

        public static IConnectionStringIdentity Statistics =
            StatisticsConnectionStringIdentity.Instance;
    }
}