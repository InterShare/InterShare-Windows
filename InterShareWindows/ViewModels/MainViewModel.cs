using InterShareWindows.Services;

namespace InterShareWindows.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(ITestService testService)
    {
        Name = testService.GetName();
    }

    public string Name { get; set; }
}