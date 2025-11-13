using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public class SecureFileUploadValidator
{
    private readonly FileUploadConfig _config;
    public SecureFileUploadValidator(FileUploadConfig config) => _config = config;

    public async Task<ValidationResult> ValidateAndSanitizeAsync(IFormFile file, string uploadFolder)
    {
        var result = new ValidationResult();

        var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? "";
        if (string.IsNullOrEmpty(ext) || !_config.AllowedExtensions.Contains(ext))
            { result.AddError($"Extension {ext} not allowed"); return result; }

        var clientMime = file.ContentType?.ToLowerInvariant() ?? "";
        if (!_config.AllowedMimePatterns.Any(p => clientMime.StartsWith(p)))
            result.AddError($"MIME {clientMime} blocked");

        using var stream = file.OpenReadStream();
        var header = new byte[32];
        var read = await stream.ReadAsync(header);
        if (read == 0) { result.AddError("Empty file"); return result; }

        var validSig = _config.MagicBytes
            .Where(kv => kv.Key == ext)
            .SelectMany(kv => kv.Value)
            .Any(sig => header.Take(sig.Length).SequenceEqual(sig));

        if (!validSig)
            result.AddError("Magic bytes mismatch - polyglot detected");

        if (!result.IsValid) return result;

        var safeName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploadFolder, safeName);
        Directory.CreateDirectory(uploadFolder);

        if (_config.ReEncodeImages && (ext is ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp" or ".bmp" or ".tiff"))
        {
            stream.Seek(0, SeekOrigin.Begin);
            using var img = await Image.LoadAsync(stream);
            img.Mutate(x => x.AutoOrient());
            await img.SaveAsync(fullPath, Image.DetectEncoder(fullPath));
        }
        else
        {
            await using var fs = File.Create(fullPath);
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(fs);
        }

        result.IsValid = true;
        result.SavedPath = fullPath;
        result.SafeFileName = safeName;
        return result;
    }
}