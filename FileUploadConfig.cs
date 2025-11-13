public class FileUploadConfig
{
    public List<string> AllowedExtensions { get; set; } = new();
    public List<string> AllowedMimePatterns { get; set; } = new();
    public Dictionary<string, List<byte[]>> MagicBytes { get; set; } = new();
    public bool ReEncodeImages { get; set; } = true;
}