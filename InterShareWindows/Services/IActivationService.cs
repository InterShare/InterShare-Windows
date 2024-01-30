using System.Threading.Tasks;

namespace InterShareWindows.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}