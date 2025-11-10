using System;
using System.Collections.Generic;
using System.Linq;

namespace Chapter01_BclChanges
{
    public class DemoClass
    {
	    public void DemoMethod()
	    {
		    IEnumerable<string> names = new[] { "ABC", "abc" };

		    // does not work in .NET Standard 2.0
			var uniqueNames = names.ToHashSet(StringComparer.CurrentCultureIgnoreCase);

			// works in .NET Standard 2.0
			var uniqueNames2 = new HashSet<string>(names, StringComparer.CurrentCultureIgnoreCase);
		}
	}
}
