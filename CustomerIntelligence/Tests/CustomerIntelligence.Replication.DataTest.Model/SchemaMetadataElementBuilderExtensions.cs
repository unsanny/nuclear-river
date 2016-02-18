using System.Linq;

using NuClear.DataTest.Metamodel;
using NuClear.Model.Common.Entities;

namespace NuClear.CustomerIntelligence.Replication.StateInitialization.Tests
{
    // todo: перенести в 2GIS.NuClear.DataTest.Metamodel
    internal static class SchemaMetadataElementBuilderExtensions
    {
        public static SchemaMetadataElementBuilder HasEntitiesFromRegistry<TSubDomain>(this SchemaMetadataElementBuilder builder, IEntityTypeMappingRegistry<TSubDomain> registry)
            where TSubDomain : ISubDomain
        {
            foreach (var type in registry.EntityMapping.Values.Union(registry.PersistenceEntities))
            {
                builder.WithFeatures(new EntityTypeFeature(type));
            }

            return builder;
        }
    }
}