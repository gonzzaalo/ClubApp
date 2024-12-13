using ClubApp.View;
using ClubApp.View.Cuotas;
using ClubApp.View.Deportistas;
using ClubApp.ViewModels.Cuotas;

namespace ClubApp
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
