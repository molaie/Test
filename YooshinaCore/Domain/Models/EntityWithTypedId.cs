namespace Yooshina.Core.Domain.Models {

	public class EntityWithTypedId<TId> : IEntityWithTypedId<TId> {
		public TId ID { get; protected set; }
	}
}
