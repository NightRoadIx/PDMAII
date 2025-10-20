using Microsoft.Maui.Controls;

namespace LaFokinCabra
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
