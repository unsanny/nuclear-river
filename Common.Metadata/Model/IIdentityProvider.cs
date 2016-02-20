using System;
using System.Linq.Expressions;

namespace NuClear.AdvancedSearch.Common.Metadata.Model
{
    /// <summary>
    /// ������������ ����� ��� ������������ ����� ����� ��������� � � ���������������.
    /// </summary>
    /// <typeparam name="TKey">��� ��������������</typeparam>
    public interface IIdentityProvider<TKey>
    {
        Expression<Func<TIdentifiable, TKey>> ExtractIdentity<TIdentifiable>()
            where TIdentifiable : IIdentifiable<TKey>;
    }
}