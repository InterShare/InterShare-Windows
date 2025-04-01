using System.Collections.Generic;

namespace InterShareWindows.Params;

public class SendParam
{
    public List<string>? FilePaths { get; set; }
    public string? ClipboardContent { get; set; }
}