using ExtraDepenencyTest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using System.Linq;
using System.Reflection;
using Yooshina.Core;
using Yooshina.Core.Domain;
using Yooshina.Core.Domain.Models;

namespace Modular.Modules.ModuleA {

	public class BaseServiceConvention : IRegistrationConvention {

		//public void Process(Type type, Registry registry) {
		//	if (!type.IsConcrete()) {
		//		return;
		//	}

		//	var interfaceTypes = type.FindInterfacesThatClose(typeof(IRepositoryWithTypedId<,>));

		//	foreach (var closedGenericType in interfaceTypes) {
		//		if (GenericsPluginGraph.CanBeCast(closedGenericType, type)) {
		//			registry.For(closedGenericType).Singleton().Use(type).Named(type.Name);
		//		}
		//	}
		//}

		public void ScanTypes(TypeSet types, Registry registry) {
			var t = types.FindTypes(TypeClassification.Concretes);
			if (!t.Any()) {
				return;
			}


			//types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed).Each(type =>
			//{
			//	// Register against all the interfaces implemented
			//	// by this concrete class
			//	type.GetInterfaces().Each(@interface => registry.For(@interface).Use(type));
			//});

			var interfaceTypes = types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed).All(type =>
			{
				// Register against all the interfaces implemented
				// by this concrete class
				type.GetInterfaces().All(@interface => {
					registry.For(@interface).Use(type);
					return true;
				});
				return true;
			});
		}
	}


	public class ModuleInitializer : IModuleInitializer {

		public void Init(IServiceCollection services, Container container, Assembly asm , IConfiguration config) {

			container.Configure(_ => {
				
				_.Scan(x => {
					x.Assembly(asm);
					x.AssemblyContainingType<AnotherTestService>();
					//x.AssemblyContainingType<ModularDbContext>();

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
