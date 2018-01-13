using Modular.Modules.Core.Infrastructure;
using Yooshina.Core.Domain;
using Yooshina.Core.Domain.Models;

namespace Modular.WebHost.Modules.Modular.Modules.Core.Infrastructure {
	public class Repository<T> : RepositoryWithTypedId<T, long>, IRepository<T>
		where T : class, IEntityWithTypedId<long> {
		public Repository(ModularDbContext context) : base(context) {
		}
	}

}
