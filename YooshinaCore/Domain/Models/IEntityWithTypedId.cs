namespace Yooshina.Core.Domain.Models {

	public interface IEntityWithTypedId<TId> {
		TId ID { get; }
	}
}
