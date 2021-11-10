using System.Linq;

namespace ViteReact.Shared.Controller
{
    public static class ControllerExtensions
    {
        public static IQueryable<TEntity> Limit<TEntity>(this IQueryable<TEntity> source, int limit)
        {
            if(limit <= 0) return source;

            return source.Take(limit);
        }
    }
}