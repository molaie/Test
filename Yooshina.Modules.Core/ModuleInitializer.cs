using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modular.Modules.Core.Infrastructure;
using Modular.Modules.Core.Models;
using Modular.Modules.Core.Services;
using Modular.WebHost.Modules.Modular.Modules.Core.Infrastructure;
using StructureMap;
using System.Reflection;
using Yooshina.Core;
using Yooshina.Core.Domain;
using Yooshina.Core.Domain.Models;

namespace Yooshina.Modules.Core {

	public class ModuleInitializer : IModuleInitializer {

		public void Init(IServiceCollection services, Container container, Assembly asm, IConfiguration config) {


			services.AddDbContext<ModularDbContext>(options =>
				options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
			);


			services.AddIdentity<User, Role>()
				.AddEntityFrameworkStores<ModularDbContext>()
				.AddDefaultTokenProviders();


			container.Configure(_ => {
				_.For(typeof(IRepository<>)).Use(typeof(Repository<>));
				_.For<IEmailSender>().Use<AuthMessageSender>();
				_.For<ISmsSender>().Use<AuthMessageSender>();
				_.Scan(x => {
					x.Assembly(asm);

					x.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
					x.ConnectImplementationsToTypesClosing(typeof(IRepositoryWithTypedId<,>));
					x.ConnectImplementationsToTypesClosing(typeof(IEntityWithTypedId<>));

					x.WithDefaultConventions();
					//x.AddAllTypesOf(typeof(IEntityWithTypedId<>));
					//x.AddAllTypesOf(typeof(EntityWithTypedId<>));
					//x.AddAllTypesOf(typeof(Entity));

					//x.Convention<BaseServiceConvention>();
					//

					//x.AddAllTypesOf(typeof(IRepository<>));
					//x.AddAllTypesOf(typeof(IRepositoryWithTypedId<,>));
					//services.AddTransient<IAnotherTestService, >();

					//x.RegisterConcreteTypesAgainstTheFirstInterface();
					//x.ConnectImplementationsToTypesClosing(typeof(IRepository<>));
					//x.ConnectImplementationsToTypesClosing(typeof(IRepositoryWithTypedId<,>));
				});
				//_.Populate(services);
			});


			//
			//services.AddTransient<ITestService, TestService>();


			//var container = new Container();
			//container.Configure(_ => {
			//	_.Scan(x => {
			//		x.TheCallingAssembly();
			//		x.AssemblyContainingType<ITestService>();
			//		x.WithDefaultConventions();

			//		//x.ConnectImplementationsToTypesClosing(typeof(IRayanService<>));
			//	});
			//	_.Populate(services);
			//});

		}
	}
}
