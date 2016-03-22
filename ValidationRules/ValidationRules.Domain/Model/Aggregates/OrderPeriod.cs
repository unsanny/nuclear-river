using System;

using NuClear.River.Common.Metadata.Model;

namespace NuClear.ValidationRules.Domain.Model.Aggregates
{
    /// <summary>
    /// ������ ������.
    /// ����� ���� �������� ������ ������ ���� ���������� � ��������� � �������� ���������� ������.
    /// </summary>
    public sealed class OrderPeriod : IAggregateValueObject
    {
        public long OrderId { get; set; }
        public long OrganizationUnitId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}