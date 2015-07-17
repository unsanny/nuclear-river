﻿using System;
using System.Collections.Generic;

using NuClear.AdvancedSearch.Replication.CustomerIntelligence.Transforming.Operations;
using NuClear.Messaging.API;
using NuClear.Messaging.API.Flows;
using NuClear.Messaging.API.Processing;

namespace NuClear.Replication.OperationsProcessing.Final
{
    public class ProjectStatisticsAggregatableMessage : IAggregatableMessage
    {
        public ProjectStatisticsAggregatableMessage()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public IMessageFlow TargetFlow { get; set; }
        public IReadOnlyCollection<StatisticsOperation> Operations { get; set; }

        public bool Equals(IMessage other)
        {
            var x = other as ProjectStatisticsAggregatableMessage;
            return x != null && Equals(x.Id, Id);
        }
    }
}