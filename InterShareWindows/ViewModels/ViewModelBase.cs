using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Services;
using Microsoft.UI.Xaml.Controls;

namespace InterShareWindows.ViewModels;

public abstract class ViewModelBase : ObservableRecipient, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected readonly INavigationService _navigationService;
    
    public readonly RelayCommand GoBackCommand;

    public ViewModelBase(INavigationService navigationService)
    {
        _navigationService = navigationService;
        
        GoBackCommand = new RelayCommand(GoBack);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private void GoBack()
    {
        _navigationService.GoBack();
    }
    
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}