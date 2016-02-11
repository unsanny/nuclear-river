using System;
using System.Collections.Generic;

using LinqToDB.Data;

using NuClear.AdvancedSearch.Common.Metadata.Elements;
using NuClear.Metamodeling.Elements;
using NuClear.Replication.Bulk.Api.Replicators;
using NuClear.Replication.Bulk.Api.Storage;

namespace NuClear.Replication.Bulk.Api.Factories
{
    public class RoutingBulkReplicatorFactory : IBulkReplicatorFactory
    {
        private readonly IReadOnlyDictionary<DataConnection, Type[]> _sourceDataConnections;
        private readonly DataConnection _targetDataConnection;

        private static readonly IReadOnlyDictionary<Type, Type> RoutingDictionary =
            new Dictionary<Type, Type>
            {
                { typeof(FactMetadata<>), typeof(FactBulkReplicatorFactory<>) },
                { typeof(AggregateMetadata<>), typeof(AggregatesBulkReplicatorFactory<>) },
                { typeof(ValueObjectMetadataElement<>), typeof(ValueObjectsBulkReplicatorFactory<>) },
                { typeof(StatisticsRecalculationMetadata<>), typeof(StatisticsBulkReplicatorFactory<>) }
            };

        public RoutingBulkReplicatorFactory(IReadOnlyDictionary<DataConnection, Type[]> sourceDataConnections, DataConnection targetDataConnection)
        {
            _sourceDataConnections = sourceDataConnections;
            _targetDataConnection = targetDataConnection;
        }

        IReadOnlyCollection<IBulkReplicator> IBulkReplicatorFactory.Create(IMetadataElement metadataElement)
        {
            var metadataElementType = metadataElement.GetType();

            Type factoryType;
            if (!RoutingDictionary.TryGetValue(metadataElementType.GetGenericTypeDefinition(), out factoryType))
            {
                throw new NotSupportedException($"Bulk replication is not supported for the mode described with {metadataElement}");
            }

            var objType = metadataElementType.GenericTypeArguments[0];
            var factory = (IBulkReplicatorFactory)Activator.CreateInstance(factoryType.MakeGenericType(objType), new LinqToDbQuery(_sourceDataConnections), _targetDataConnection);
            return factory.Create(metadataElement);
        }

        public void Dispose()
        {
            foreach (var sourceDataConnection in _sourceDataConnections)
            {
                sourceDataConnection.Key.Dispose();
            }
            _targetDataConnection.Dispose();
        }
    }
}