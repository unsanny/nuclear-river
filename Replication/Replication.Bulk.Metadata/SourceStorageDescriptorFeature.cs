using LinqToDB.Mapping;

using NuClear.Model.Common.Entities;
using NuClear.Storage.API.ConnectionStrings;

namespace NuClear.Replication.Bulk.Metadata
{
    public class SourceStorageDescriptorFeature : StorageDescriptorFeature
    {
        public SourceStorageDescriptorFeature(IConnectionStringIdentity connectionStringName, MappingSchema mappingSchema, IEntityTypeMappingRegistry<ISubDomain> registry)
            : base(ReplicationDirection.From, connectionStringName, mappingSchema)
        {
            Registry = registry;
        }

        public IEntityTypeMappingRegistry<ISubDomain> Registry { get; }
    }
}