[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(IFormFile file,
        [FromServices] SecureFileUploadValidator validator,
        IWebHostEnvironment env)
    {
        var folder = Path.Combine(env.ContentRootPath, "uploads");
        var result = await validator.ValidateAndSanitizeAsync(file, folder);
        return result.IsValid
            ? Ok(new { file = result.SafeFileName })
            : BadRequest(new { errors = result.Errors });
    }
}
