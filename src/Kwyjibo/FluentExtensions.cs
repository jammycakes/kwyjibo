using Kwyjibo.Fluent;

namespace Kwyjibo
{
    public static class FluentExtensions
    {
        public static IForContextClause Enabled(this IForContextClause clause)
            => clause.SetStatus(Status.Enabled);

        public static IForContextClause Disabled(this IForContextClause clause)
            => clause.SetStatus(Status.Disabled);
    }
}
