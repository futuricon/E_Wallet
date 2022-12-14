using System.Text.Json.Serialization;

namespace E_Wallet.Application.Shared;

public class DataResult
{
    private static readonly DataResult _success = new();

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Error { get; init; }
    public bool Success => string.IsNullOrEmpty(Error);

    public static DataResult CreateSuccess() => _success;
    public static DataResult CreateError(string error) => new() { Error = error };
}

public class DataResult<T> : DataResult
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Value { get; set; }

    public static DataResult<T> CreateSuccess(T result) => new() { Value = result };
    public new static DataResult<T> CreateError(string error) => new() { Error = error };
}