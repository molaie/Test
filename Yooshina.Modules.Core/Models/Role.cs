using Microsoft.AspNetCore.Identity;
using Yooshina.Core.Domain.Models;

namespace Modular.Modules.Core.Models {
	public class Role : IdentityRole<long>, IEntityWithTypedId<long>
    {
        public Role()
        {
        }


		public long ID { get; set; }

		public Role(string name)
        {
            Name = name;
        }
    }
}
