using System.Threading.Tasks;

namespace InterShareWindows.Activation;

public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}