using System;
using Microsoft.AspNetCore.Identity;
using Yooshina.Core.Domain.Models;

namespace Modular.Modules.Core.Models {
	public class User : IdentityUser<long>, IEntityWithTypedId<long>
    {
        public User()
        {
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.Now;
        }

		public long ID { get; set; }

		public string FullName { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

	}
}
