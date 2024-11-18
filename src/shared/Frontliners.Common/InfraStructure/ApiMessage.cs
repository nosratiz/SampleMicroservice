namespace Frontliners.Common.InfraStructure;

public sealed record ApiMessage(string message);

public sealed record ApiError(Error data);

public sealed record Error(string message, IDictionary<string, string[]>? errors);