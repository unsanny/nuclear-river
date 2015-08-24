using LinqToDB.Mapping;

using NuClear.AdvancedSearch.Replication.CustomerIntelligence.Model;

namespace NuClear.AdvancedSearch.Replication.CustomerIntelligence.Data
{
    public static partial class Schema
    {
        private const string CustomerIntelligenceSchema = "CustomerIntelligence";
        
        public static MappingSchema CustomerIntelligence
        {
            get
            {
                var schema = new MappingSchema();
                var config = schema.GetFluentMappingBuilder();

                config.HasAttribute<FirmCategoryStatistics>(new TableAttribute { Schema = CustomerIntelligenceSchema, Name = "FirmCategory", IsColumnAttributeRequired = false });

                config.Entity<CategoryGroup>().HasSchemaName(CustomerIntelligenceSchema).Property(x => x.Id).IsPrimaryKey();
                config.Entity<Client>().HasSchemaName(CustomerIntelligenceSchema).Property(x => x.Id).IsPrimaryKey();
                config.Entity<ClientContact>().HasSchemaName(CustomerIntelligenceSchema)
                    .Property(x => x.ContactId).IsPrimaryKey()
                    .Property(x => x.ClientId).IsPrimaryKey();
                config.Entity<Firm>().HasSchemaName(CustomerIntelligenceSchema).HasTableName("FirmBase").Property(x => x.Id).IsPrimaryKey();
                config.Entity<FirmActivity>().HasSchemaName(CustomerIntelligenceSchema).Property(x => x.FirmId).IsPrimaryKey();
                config.Entity<FirmBalance>().HasSchemaName(CustomerIntelligenceSchema)
                    .Property(x => x.AccountId).IsPrimaryKey()
                    .Property(x => x.FirmId).IsPrimaryKey();
                config.Entity<FirmCategory>().HasSchemaName(CustomerIntelligenceSchema)
                    .Property(x => x.CategoryId).IsPrimaryKey()
                    .Property(x => x.FirmId).IsPrimaryKey();
                config.Entity<Project>().HasSchemaName(CustomerIntelligenceSchema).Property(x => x.Id).IsPrimaryKey();
                config.Entity<ProjectCategory>().HasSchemaName(CustomerIntelligenceSchema)
                    .Property(x => x.ProjectId).IsPrimaryKey()
                    .Property(x => x.CategoryId).IsPrimaryKey();
                config.Entity<Territory>().HasSchemaName(CustomerIntelligenceSchema).Property(x => x.Id).IsPrimaryKey();
                config.Entity<FirmCategoryStatistics>()
                    .Property(x => x.ProjectId).IsNotColumn()
                    .Property(x => x.FirmId).IsPrimaryKey()
                    .Property(x => x.CategoryId).IsPrimaryKey();

                return schema;
            }
        }
    }
}