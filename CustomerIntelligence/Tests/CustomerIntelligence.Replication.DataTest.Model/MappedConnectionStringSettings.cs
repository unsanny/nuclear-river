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
        private readonly IConnectionStringSettings _connectionStringSettings;

        public MappedConnectionStringSettings(
            IConnectionStringSettings connectionStringSettings)
        {
            _connectionStringSettings = connectionStringSettings;
        }

        public static IConnectionStringSettings CreateMappedSettings(IConnectionStringSettings baseSettings, ActMetadataElement actMetadata, IDictionary<string, SchemaMetadataElement> schemaMetadata)
        {
            return new MappedConnectionStringSettings(baseSettings);
        }

        public string GetConnectionString(IConnectionStringIdentity identity)
        {
            return _connectionStringSettings.GetConnectionString(identity);
        }

        public IReadOnlyDictionary<IConnectionStringIdentity, string> AllConnectionStrings
            => _connectionStringSettings.AllConnectionStrings;
    }
}