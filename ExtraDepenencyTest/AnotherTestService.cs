using System;
using System.Collections.Generic;
using System.Text;

namespace ExtraDepenencyTest {
	public class AnotherTestService : IAnotherTestService {
		public string Test() {
			return "Other service outside modules";
		}
	}
}
