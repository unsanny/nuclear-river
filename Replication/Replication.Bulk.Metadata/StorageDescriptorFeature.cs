using LinqToDB.Mapping;

using NuClear.Metamodeling.Elements.Aspects.Features;
using NuClear.Storage.API.ConnectionStrings;

namespace NuClear.Replication.Bulk.Metadata
{
    public abstract class StorageDescriptorFeature : IMetadataFeature
    {
        protected StorageDescriptorFeature(ReplicationDirection direction, IConnectionStringIdentity connectionStringName, MappingSchema mappingSchema)
        {
            Direction = direction;
            ConnectionStringName = connectionStringName;
            MappingSchema = mappingSchema;
        }

        public ReplicationDirection Direction { get; }
        public IConnectionStringIdentity ConnectionStringName { get; }
        public MappingSchema MappingSchema { get; }
    }
}