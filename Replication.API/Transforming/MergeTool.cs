using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using NuClear.Telemetry.Probing;

namespace NuClear.AdvancedSearch.Replication.API.Transforming
{
    public interface IMergeResult<out T>
    {
        IEnumerable<T> Difference { get; }
        IEnumerable<T> Intersection { get; }
        IEnumerable<T> Complement { get; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public static class MergeTool
    {
        public static MergeResult<T> Merge<T>(IEnumerable<T> data1, IEnumerable<T> data2, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            using (Probe.Create("Merging", typeof(T).Name))
            {
                HashSet<T> set1;
                using (Probe.Create("Query source"))
                {
                    set1 = new HashSet<T>(data1, comparer);
                }

                HashSet<T> set2;
                using (Probe.Create("Query target"))
                {
                    set2 = new HashSet<T>(data2, comparer);
                }

                // NOTE: avoiding enumerable extensions to reuse hashset performance
                var difference = set1.Where(x => !set2.Contains(x));
                var intersection = set1.Where(x => set2.Contains(x));
                var complement = set2.Where(x => !set1.Contains(x));

                return new MergeResult<T>(difference, intersection, complement);
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class MergeResult<T> : IMergeResult<T>
    {
        public IEnumerable<T> Difference { get; private set; }
        public IEnumerable<T> Intersection { get; private set; }
        public IEnumerable<T> Complement { get; private set; }

        public MergeResult(IEnumerable<T> difference, IEnumerable<T> intersection, IEnumerable<T> complement)
        {
            Difference = difference;
            Intersection = intersection;
            Complement = complement;
        }

        public MergeResult(IMergeResult<T> mergeResult)
        {
            Difference = mergeResult.Difference;
            Intersection = mergeResult.Intersection;
            Complement = mergeResult.Complement;
        }
    }
}