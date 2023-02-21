using System.ComponentModel.DataAnnotations;

namespace MR.Dispatcher
{
    public class AuthRequestCreate
    {
        [Required] public string Hi { get; init; } = string.Empty;
    }

    public class AuthRequestConnect
    {
        [Required] public string Hi { get; init; } = string.Empty;
        [Required] public string UUID { get; init; } = string.Empty;
    }
}
