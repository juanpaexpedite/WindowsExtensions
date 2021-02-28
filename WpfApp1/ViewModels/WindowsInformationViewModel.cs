using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WindowsExtensions.Models;
using WindowsExtensions.Services;
using WindowsExtensions.Views;

namespace WindowsExtensions.ViewModels
{
    public class WindowsInformationViewModel : BindableBase
    {
        public ObservableCollection<WindowInformation> Windows { get; } = new ObservableCollection<WindowInformation>();

        public WindowsInformationViewModel()
        {
            PlaceAppOnTopCommand = Initialize_PlaceAppOnTopCommand();
            PlaceAppNormalCommand = Initialize_PlaceAppNormalCommand();
            FocusAppCommand = Initialize_FocusAppCommand();
            CloseAppCommand = Initialize_CloseAppCommand();
            DonateCommand = Initialize_DonateCommand();

            StartTimer();
        }

        private void StartTimer()
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(2);
            dt.Tick += (s, e) =>
            {
                RefreshWindowsEnumeration();
            };
            dt.Start();
        }

        private void RefreshWindowsEnumeration()
        {
            List<WindowInformation> windowsinformationupdated = new List<WindowInformation>();

            WindowsService.EnumerateWindows((wi) =>
            {
                windowsinformationupdated.Add(wi);
                if(WindowsService.IsLastWindow(wi.HWnd))
                {
                    UpdateList(windowsinformationupdated);
                }
            });
        }

        private void UpdateList(List<WindowInformation> list)
        {
            var toremove = Windows.Where(w => !list.Any(l => l.HWnd == w.HWnd)).ToList();

            foreach(var w in toremove)
            {
                Windows.Remove(w);
            }

            var toadd = list.Where(w => !Windows.Any(l => l.HWnd == w.HWnd)).ToList();

            foreach(var w in toadd)
            {
                Windows.Add(w);
            }

            UpdateNames();
        }

        private void UpdateNames()
        {
            foreach (var w in Windows)
            {
                w.Name =  WindowsService.GetWindowName(w.HWnd);
            }
        }

        private WindowInformation selectedwindow;

        public WindowInformation SelectedWindow
        {
            get { return selectedwindow; }
            set
            {
                SetProperty(ref selectedwindow, value);
            }
        }

        private WindowInformation lastWindowOnTop = null;

        public ICommand PlaceAppOnTopCommand { get; }
        private ICommand Initialize_PlaceAppOnTopCommand()
        {
            return new DelegateCommand(() =>
            {
                if (selectedwindow != null)
                {
                    lastWindowOnTop = selectedwindow;
                    selectedwindow.OnTop = true;
                    WindowsService.Focus(selectedwindow.HWnd);
                    WindowsService.SetOnTop(selectedwindow.HWnd);
                }
            });
        }

        public ICommand PlaceAppNormalCommand { get; }
        private ICommand Initialize_PlaceAppNormalCommand()
        {
            return new DelegateCommand(() =>
            {
                if (selectedwindow != null)
                {
                    selectedwindow.OnTop = false;
                    WindowsService.RemoveFromTop(selectedwindow.HWnd);
                }
            });
        }

        public ICommand FocusAppCommand { get; }
        private ICommand Initialize_FocusAppCommand()
        {
            return new DelegateCommand(() =>
            {
                if (selectedwindow != null)
                {
                    WindowsService.Focus(selectedwindow.HWnd);
                }
            });
        }

        public ICommand CloseAppCommand { get; }
        private ICommand Initialize_CloseAppCommand()
        {
            return new DelegateCommand(() =>
            {
                if (selectedwindow != null)
                {
                    WindowsService.Close(selectedwindow.HWnd);
                }
            });
        }

        #region Donation
        public ICommand DonateCommand { get; }

        private ICommand Initialize_DonateCommand()
        {
            return new DelegateCommand(() =>
            {
                WPFCore.WPFService.ShowDialogBox<DonateView>();
            });
        }
        #endregion
    }
}
