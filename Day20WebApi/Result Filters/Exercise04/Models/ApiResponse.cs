namespace Exercise04.Models
{
    public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ApiMetadata Metadata { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}
}