﻿using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

using Microsoft.Practices.Unity;

using NuClear.AdvancedSearch.Replication.CustomerIntelligence.Data.Context;
using NuClear.AdvancedSearch.Replication.CustomerIntelligence.Data.Context.Implementation;
using NuClear.AdvancedSearch.Replication.CustomerIntelligence.Transforming;
using NuClear.AdvancedSearch.Replication.Data;
using NuClear.DI.Unity.Config;
using NuClear.Replication.OperationsProcessing.Transports.SQLStore;

using Schema = NuClear.AdvancedSearch.Replication.CustomerIntelligence.Data.Schema;

namespace NuClear.AdvancedSearch.Replication.EntryPoint.DI
{
    using TransportSchema = NuClear.Replication.OperationsProcessing.Transports.SQLStore.Schema;

    public static partial class Bootstrapper
    {
        private static IUnityContainer ConfigureLinq2Db(this IUnityContainer container)
        {
            return container
                .RegisterDataContext(Scope.Erm, Connection.Erm, Schema.Erm)
                .RegisterDataContext(Scope.Facts, Connection.CustomerIntelligence, Schema.Facts)
                .RegisterDataContext(Scope.CustomerIntelligence, Connection.CustomerIntelligence, Schema.CustomerIntelligence)
                .RegisterDataContext(Scope.Transport, Connection.CustomerIntelligence, TransportSchema.Transport)

                .RegisterType<IDataMapper, DataMapper>(Scope.Facts, Lifetime.PerScope, new InjectionConstructor(new ResolvedParameter<IDataContext>(Scope.Facts)))
                .RegisterType<IDataMapper, DataMapper>(Scope.CustomerIntelligence, Lifetime.PerScope, new InjectionConstructor(new ResolvedParameter<IDataContext>(Scope.CustomerIntelligence)))

                .RegisterType<IErmContext, ErmContext>(Lifetime.PerScope, new InjectionConstructor(new ResolvedParameter<IDataContext>(Scope.Erm)))

                .RegisterType<FactsContext>(Lifetime.PerScope, new InjectionConstructor(new ResolvedParameter<IDataContext>(Scope.Facts)))
                .RegisterType<FactsTransformationContext>(Lifetime.PerScope)

                .RegisterType<CustomerIntelligenceContext>(Lifetime.PerScope, new InjectionConstructor(new ResolvedParameter<IDataContext>(Scope.CustomerIntelligence)))
                .RegisterType<CustomerIntelligenceTransformationContext>(Lifetime.PerScope, new InjectionConstructor(new ResolvedParameter<FactsContext>()))


                .RegisterType<FactsTransformation>(Lifetime.PerScope,
                                                   new InjectionConstructor(
                                                       new ResolvedParameter<FactsTransformationContext>(),
                                                       new ResolvedParameter<FactsContext>(),
                                                       new ResolvedParameter<IDataMapper>(Scope.Facts)))

                .RegisterType<CustomerIntelligenceTransformation>(Lifetime.PerScope,
                                                   new InjectionConstructor(
                                                       new ResolvedParameter<CustomerIntelligenceTransformationContext>(),
                                                       new ResolvedParameter<CustomerIntelligenceContext>(),
                                                       new ResolvedParameter<IDataMapper>(Scope.CustomerIntelligence)))

                .RegisterType<SqlStoreSender>(Lifetime.PerScope, new InjectionConstructor(new ResolvedParameter<IDataContext>(Scope.Transport)))
                .RegisterType<SqlStoreReceiver>(Lifetime.PerScope, new InjectionConstructor(new ResolvedParameter<IDataContext>(Scope.Transport)));
        }

        private static IUnityContainer RegisterDataContext(this IUnityContainer container, string scope, string connection, MappingSchema schema)
        {
            return container.RegisterType<IDataContext, DataConnection>(scope, Lifetime.PerScope, new InjectionFactory(c => new DataConnection(connection).AddMappingSchema(schema)));
        }

        private static class Scope
        {
            public const string Erm = "Erm";
            public const string Facts = "Facts";
            public const string CustomerIntelligence = "CustomerIntelligence";
            public const string Transport = "Transport";
        }

        private static class Connection
        {
            public const string Erm = "Erm";
            public const string CustomerIntelligence = "CustomerIntelligence";
        }
    }
}