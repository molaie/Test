using Microsoft.EntityFrameworkCore;
using Modular.Modules.ModuleA.Models;
using Yooshina.Core;

namespace Modular.Modules.ModuleA.Infrastructure {
	public class ModuleACustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {

			modelBuilder.Entity<Sample>()
			   .ToTable("Filan_sample");

			modelBuilder.Entity<Sample>()
                .Property(x => x.Name).HasColumnName("TestName");
        }
    }
}
