using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WindowsExtensions.Models
{
    public class WindowInformation : BindableBase
    {
        public IntPtr HWnd;

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
            }
        }

        private bool ontop = false;

        public bool OnTop
        {
            get { return ontop; }
            set
            {
                SetProperty(ref ontop, value);
            }
        }

        public BitmapSource Icon { get; set; }

        public WindowInformation() { }

        public WindowInformation(IntPtr hwnd, string name, BitmapSource icon)
        {
            HWnd = hwnd;
            Name = name;
            Icon = icon;
        }
    }
}
