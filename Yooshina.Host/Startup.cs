using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Yooshina.Core;
using Yooshina.Host.Extensions;

namespace Yooshina.Host {
	public class Startup {


		private readonly IList<ModuleInfo> modules = new List<ModuleInfo>();
		private readonly IHostingEnvironment _hostingEnvironment;


		public Startup(IConfiguration configuration, IHostingEnvironment env) {
			_hostingEnvironment = env;
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public IServiceProvider ConfigureServices(IServiceCollection services) {

			LoadInstalledModules();


			services.Configure<RazorViewEngineOptions>(options => {
				options.ViewLocationExpanders.Add(new ModuleViewLocationExpander());
			});

			var mvcBuilder = services.AddMvc();

			var container = new Container();

			var moduleInitializerInterface = typeof(IModuleInitializer);
			foreach (var module in modules) {
				// Register controllers from modules
				mvcBuilder.AddApplicationPart(module.Assembly);
				// Register dependency in modules
				var moduleInitializerType = module.Assembly.GetTypes().Where(x => typeof(IModuleInitializer).IsAssignableFrom(x)).FirstOrDefault();
				if (moduleInitializerType != null && moduleInitializerType != typeof(IModuleInitializer)) {
					var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
					moduleInitializer.Init(services, container, module.Assembly, Configuration);
				}
			}


			container.Configure(_ => {
				_.Scan(x => {
					x.TheCallingAssembly();
					x.AssembliesFromApplicationBaseDirectory();
					
					x.WithDefaultConventions();
					
				});
				_.Populate(services);
			});
			return container.GetInstance<IServiceProvider>();
		}


		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			} else {
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			foreach (var module in modules) {
				var wwwrootDir = new DirectoryInfo(Path.Combine(module.Path, "wwwroot"));
				if (!wwwrootDir.Exists) {
					continue;
				}

				app.UseStaticFiles(new StaticFileOptions() {
					FileProvider = new PhysicalFileProvider(wwwrootDir.FullName),
					RequestPath = new PathString("/" + module.ShortName)
				});
			}

			app.UseIdentity();


			app.UseMvc(routes => {
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}


		private void LoadInstalledModules() {
			var moduleRootFolder = new DirectoryInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "Modules"));
			var moduleFolders = moduleRootFolder.GetDirectories();

			foreach (var moduleFolder in moduleFolders) {
				var binFolder = new DirectoryInfo(Path.Combine(moduleFolder.FullName, "bin"));
				if (!binFolder.Exists) {
					continue;
				}

				foreach (var file in binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories)) {
					Assembly assembly = null;
					try {
						assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
					} catch (FileLoadException ex) {
						if (ex.Message == "Assembly with same name is already loaded") {
							// Get loaded assembly
							assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));
						} else {
							throw;
						}
					}

					if (assembly.FullName.Contains(moduleFolder.Name) && assembly.FullName.ToLower().Contains(".modules.")) {
						modules.Add(new ModuleInfo { Name = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.FullName });
					}
				}
			}

			GlobalConfiguration.Modules = modules;
		}



	}
}
