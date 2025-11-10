using Microsoft.Extensions.Logging;

namespace Chapter02_SourceGenerators;

public partial class GeneratedLog
{

	[LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Processing order {OrderId}")]
	public static partial void ProcessingOrder(ILogger logger, Guid orderId);

	[LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "Error processing order {OrderId}: {ErrorMessage}")]
	public static partial void ProcessingOrderError(ILogger logger, Guid orderId, string errorMessage);

}