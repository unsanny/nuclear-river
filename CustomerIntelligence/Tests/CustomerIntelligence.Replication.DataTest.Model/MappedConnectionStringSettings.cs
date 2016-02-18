using System.Collections.Generic;
using System.Linq;

using NuClear.CustomerIntelligence.Replication.StateInitialization.Tests.Identitites.Connections;
using NuClear.CustomerIntelligence.Storage.Identitites.Connections;
using NuClear.DataTest.Metamodel;
using NuClear.DataTest.Metamodel.Dsl;
using NuClear.Storage.API.ConnectionStrings;

namespace NuClear.CustomerIntelligence.Replication.StateInitialization.Tests
{
    public sealed class MappedConnectionStringSettings : IConnectionStringSettings
    {
        private static readonly IReadOnlyDictionary<IConnectionStringIdentity, IConnectionStringIdentity> Mapping =
            new Dictionary<IConnectionStringIdentity, IConnectionStringIdentity>
            {
                { ErmConnectionStringIdentity.Instance, ErmTestConnectionStringIdentity.Instance },
                { FactsConnectionStringIdentity.Instance, FactsTestConnectionStringIdentity.Instance },
                { BitConnectionStringIdentity.Instance, BitTestConnectionStringIdentity.Instance },
                { CustomerIntelligenceConnectionStringIdentity.Instance, CustomerIntelligenceTestConnectionStringIdentity.Instance },
                { StatisticsConnectionStringIdentity.Instance, StatisticsTestConnectionStringIdentity.Instance },
            };

        private readonly IConnectionStringSettings _connectionStringSettings;
        private readonly IReadOnlyDictionary<IConnectionStringIdentity, IConnectionStringIdentity> _mapping;

        public MappedConnectionStringSettings(
            IConnectionStringSettings connectionStringSettings,
            IReadOnlyDictionary<IConnectionStringIdentity, IConnectionStringIdentity> mapping)
        {
            _connectionStringSettings = connectionStringSettings;
            _mapping = mapping;
        }

        public static IConnectionStringSettings CreateMappedSettings(IConnectionStringSettings baseSettings, ActMetadataElement actMetadata, IDictionary<string, SchemaMetadataElement> schemaMetadata)
        {
            return new MappedConnectionStringSettings(baseSettings, Mapping);
        }

        public string GetConnectionString(IConnectionStringIdentity identity)
        {
            return _connectionStringSettings.GetConnectionString(_mapping[identity]);
        }

        public IReadOnlyDictionary<IConnectionStringIdentity, string> AllConnectionStrings
            => _connectionStringSettings.AllConnectionStrings.ToDictionary(x => _mapping[x.Key], x => x.Value);
    }
}