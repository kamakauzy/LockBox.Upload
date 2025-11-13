public class ValidationResult
{
    public bool IsValid { get; set; }
    public string? SavedPath { get; set; }
    public string? SafeFileName { get; set; }
    public List<string> Errors { get; } = new();
    public void AddError(string msg) => Errors.Add(msg);
}