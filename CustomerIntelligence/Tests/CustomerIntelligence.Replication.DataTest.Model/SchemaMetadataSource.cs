using System;
using System.Collections.Generic;

using NuClear.CustomerIntelligence.Domain.Contexts;
using NuClear.CustomerIntelligence.Replication.StateInitialization.Tests.Identitites.Connections;
using NuClear.CustomerIntelligence.Storage;
using NuClear.CustomerIntelligence.Storage.Identitites.Connections;
using NuClear.DataTest.Metamodel;
using NuClear.Metamodeling.Elements;
using NuClear.Metamodeling.Provider.Sources;

namespace NuClear.CustomerIntelligence.Replication.StateInitialization.Tests
{
    public class SchemaMetadataSource : MetadataSourceBase<SchemaMetadataIdentity>
    {
        private static readonly SchemaMetadataElement Erm = SchemaMetadataElement.Config
            .For(ContextName.Erm)
            .HasConnectionString<ErmConnectionStringIdentity>()
            .HasMasterConnectionString<ErmMasterConnectionStringIdentity>()
            .HasSchema(Schema.Erm)
            .HasEntitiesFromRegistry(EntityTypeMap.CreateErmContext());

        private static readonly SchemaMetadataElement Facts = SchemaMetadataElement.Config
            .For(ContextName.Facts)
            .HasConnectionString<FactsConnectionStringIdentity>()
            .HasSchema(Schema.Facts)
            .HasEntitiesFromRegistry(EntityTypeMap.CreateFactsContext());

        private static readonly SchemaMetadataElement CustomerIntelligence = SchemaMetadataElement.Config
            .For(ContextName.CustomerIntelligence)
            .HasConnectionString<CustomerIntelligenceConnectionStringIdentity>()
            .HasSchema(Schema.CustomerIntelligence)
            .HasEntitiesFromRegistry(EntityTypeMap.CreateCustomerIntelligenceContext());

        private static readonly SchemaMetadataElement Bit = SchemaMetadataElement.Config
            .For(ContextName.Bit)
            .HasConnectionString<BitConnectionStringIdentity>()
            .HasSchema(Schema.Bit)
            .HasEntitiesFromRegistry(EntityTypeMap.CreateBitFactsContext());

        private static readonly SchemaMetadataElement Statistics = SchemaMetadataElement.Config
            .For(ContextName.Statistics)
            .HasConnectionString<StatisticsConnectionStringIdentity>()
            .HasSchema(Schema.Statistics)
            .HasEntitiesFromRegistry(EntityTypeMap.CreateStatisticsContext());

        public SchemaMetadataSource()
        {
            Metadata = new Dictionary<Uri, IMetadataElement>
                        {
                            { Erm.Identity.Id, Erm },
                            { Facts.Identity.Id, Facts },
                            { CustomerIntelligence.Identity.Id, CustomerIntelligence },
                            { Bit.Identity.Id, Bit },
                            { Statistics.Identity.Id, Statistics },
                        };
        }

        public override IReadOnlyDictionary<Uri, IMetadataElement> Metadata { get; }
    }
}
