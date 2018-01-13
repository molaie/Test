using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using System.Reflection;

namespace Yooshina.Core {
	public interface IModuleInitializer {
		void Init(IServiceCollection serviceCollection, Container container, Assembly asm, IConfiguration config);
	}
}
