﻿using System;
using System.Linq;

using Moq;

using NuClear.AdvancedSearch.Replication.API.Transforming;
using NuClear.AdvancedSearch.Replication.CustomerIntelligence.Transforming;
using NuClear.Storage.Readings;
using NuClear.Storage.Writings;

namespace NuClear.AdvancedSearch.Replication.Tests
{
    public class VerifiableSourceChangesApplierFactory : ISourceChangesApplierFactory
    {
        private readonly Action<IRepository> _onRepositoryCreated;

        public VerifiableSourceChangesApplierFactory(Action<IRepository> onRepositoryCreated)
        {
            _onRepositoryCreated = onRepositoryCreated;
        }

        public ISourceChangesApplier Create(ErmFactInfo factInfo, IQuery sourceQuery, IQuery destQuery)
        {
            var repositoryType = typeof(IRepository<>).MakeGenericType(factInfo.FactType);
            var repositoryInstance = (IRepository)DynamicMock(repositoryType);

            _onRepositoryCreated(repositoryInstance);

            var applierType = typeof(SourceChangesApplier<>).MakeGenericType(factInfo.FactType);
            return (ISourceChangesApplier)Activator.CreateInstance(applierType, factInfo, sourceQuery, destQuery, repositoryInstance);
        }

        private static object DynamicMock(Type type)
        {
            var mock = typeof(Mock<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            return mock.GetType().GetProperties().Single(f => f.Name == "Object" && f.PropertyType == type).GetValue(mock, new object[] { });
        }
    }
}