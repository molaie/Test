using System.Linq;
using Yooshina.Core.Domain.Models;

namespace Yooshina.Core.Domain {
	public interface IRepositoryWithTypedId<T, in TId> where T : IEntityWithTypedId<TId>
    {
        IQueryable<T> Query();

        void Add(T entity);

        void SaveChange();

        void Remove(T entity);
    }
}
