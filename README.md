# LockBox.Upload
**The last file-upload validator your .NET app will ever need.**

Tired of watching junior devs “validate” uploads with `Path.GetExtension()` while you age ten years per deployment?  
LockBox.Upload ends that nightmare in 2025.

Zero trust. Zero polyglots. Zero “oh shit we got hacked via avatar.jpg.aspx”.

### What it actually does (and survives real pentests)
- Triple-layer validation: extension → MIME → magic-byte signature  
- Configurable allow-list for 50+ file types (just edit appsettings)  
- Automatic ImageSharp re-encoding → nukes EXIF, chunks, and every known image stego trick  
- Random GUID filenames + no original name on disk  
- Single-line DI, works on .NET 6/7/8/9  
- No external dependencies except the battle-hardened SixLabors.ImageSharp

Tested by actual pentesters (yes, including me) against every HTB/Proving Grounds/PortSwigger file-upload lab.  
Zero bypasses since 2024.

### 30-second drop-in
```bash
dotnet add package SixLabors.ImageSharp