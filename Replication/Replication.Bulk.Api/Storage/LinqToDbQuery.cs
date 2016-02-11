using System;
using System.Collections.Generic;
using System.Linq;

using LinqToDB.Data;

using NuClear.Storage.API.Readings;
using NuClear.Storage.API.Specifications;

namespace NuClear.Replication.Bulk.Api.Storage
{
    public sealed class LinqToDbQuery : IQuery
    {
        private readonly IReadOnlyDictionary<DataConnection, Type[]> _dataContexts;
        private readonly IDictionary<Type, DataConnection> _dataContextsCache;

        public LinqToDbQuery(IReadOnlyDictionary<DataConnection, Type[]> dataContexts)
        {
            _dataContexts = dataContexts;
            _dataContextsCache = new Dictionary<Type, DataConnection>();
        }

        public string LastQuery => string.Join(Environment.NewLine, _dataContexts.Select(x => x.Key.LastQuery));

        public IQueryable For(Type objType)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> For<T>() where T : class
        {
            return FindDataConnection(typeof(T)).GetTable<T>();
        }

        public IQueryable<T> For<T>(FindSpecification<T> findSpecification) where T : class
        {
            return FindDataConnection(typeof(T)).GetTable<T>().Where(findSpecification);
        }

        public DataConnection FindDataConnection(Type type)
        {
            DataConnection connection;
            if (!_dataContextsCache.TryGetValue(type, out connection))
            {
                var dataConnections = _dataContexts.Where(x => x.Value.Any(containedType => containedType == type)).ToArray();
                if (dataConnections.Length == 0)
                {
                    throw new ArgumentException($"Type {type.Name} not found in contexts", nameof(type));
                }

                if (dataConnections.Length > 1)
                {
                    throw new ArgumentException($"Found multiple occurrence of type {type.Name} in contexts", nameof(type));
                }

                connection = dataConnections.Single().Key;
                _dataContextsCache.Add(type, connection);
            }

            return connection;
        }
    }
}