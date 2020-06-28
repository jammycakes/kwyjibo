using Kwyjibo.Fluent;

namespace Kwyjibo
{
    public static class FluentExtensions
    {
        public static IForContextClause Enable(this IForContextClause clause)
            => clause.SetStatus(Status.Enabled);

        public static IForContextClause Disable(this IForContextClause clause)
            => clause.SetStatus(Status.Disabled);
    }
}
