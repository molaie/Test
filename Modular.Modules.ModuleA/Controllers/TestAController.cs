using ExtraDepenencyTest;
using Microsoft.AspNetCore.Mvc;
using Modular.Modules.ModuleA.Models;
using Modular.Modules.ModuleA.Services;
using StructureMap;
using Yooshina.Core.Domain;

namespace Modular.Modules.ModuleA.Controllers {

	public class TestAController : Controller {

		private ITestService _testService;
		private IAnotherTestService _anotherTestService;
		private IRepository<Sample> _sampleRepository;
		private readonly IContainer _Container;

		public TestAController(ITestService testService, IAnotherTestService anotherTestService, IContainer container
			//, IRepository<Sample> sampleRepository
			) {
			_testService = testService;
			_anotherTestService = anotherTestService;
			_Container = container;
			//_sampleRepository = sampleRepository;
		}

		public IActionResult Index() {
			ViewBag.TestData = _testService.Test();
			ViewBag.AnotherTestData = _anotherTestService.Test();


			//Container c = new Container();
			//c.WhatDoIHave();

			var x = _Container.GetInstance<ITestService>();

			IRepository<Sample> _sampleRepository = _Container.GetInstance<IRepository<Sample>>();
			if (null == _sampleRepository) {
				return View();
			}

			var sample = new Sample { Name = "Name test", Description = "Decription Test" };
			_sampleRepository.Add(sample);
			_sampleRepository.SaveChange();

			return View();
		}


		public ViewResult Test2() {
			ViewBag.TestData = _testService.Test();
			ViewBag.AnotherTestData = _anotherTestService.Test();

			var sample = new Sample { Name = "Name test", Description = "Decription Test" };
			//_sampleRepository.Add(sample);
			//_sampleRepository.SaveChange();

			return View();
		}



	}
}
