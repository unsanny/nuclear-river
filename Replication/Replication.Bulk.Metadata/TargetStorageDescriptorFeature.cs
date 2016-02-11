using LinqToDB.Mapping;

using NuClear.Storage.API.ConnectionStrings;

namespace NuClear.Replication.Bulk.Metadata
{
    public class TargetStorageDescriptorFeature : StorageDescriptorFeature
    {
        public TargetStorageDescriptorFeature(IConnectionStringIdentity connectionStringName, MappingSchema mappingSchema)
            : base(ReplicationDirection.To, connectionStringName, mappingSchema)
        {
        }
    }
}