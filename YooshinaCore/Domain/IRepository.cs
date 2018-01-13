using Yooshina.Core.Domain.Models;

namespace Yooshina.Core.Domain {
	public interface IRepository<T> : IRepositoryWithTypedId<T, long> where T : IEntityWithTypedId<long> {
	}
}
