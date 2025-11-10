using System.Text.RegularExpressions;

namespace Chapter02_SourceGenerators;

public partial class GeneratedRegularExpression
{

	[GeneratedRegex("abc|def", RegexOptions.IgnoreCase, "en-US")]
	private static partial Regex GeneratedRegex();

}