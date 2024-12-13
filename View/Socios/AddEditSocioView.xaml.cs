using ClubApp.Models;
using ClubApp.ViewModels.Socios;

namespace ClubApp.View.Socios
{
    public partial class AddEditSocioView : ContentPage
    {
        private AddEditSocioViewModel viewModel;

        public AddEditSocioView()
        {
            InitializeComponent();
            viewModel = new AddEditSocioViewModel();
            BindingContext = viewModel;
        }

        public AddEditSocioView(Socio socio)
        {
            InitializeComponent();
            viewModel = new AddEditSocioViewModel();
            BindingContext = viewModel;

            if (socio != null)
            {
                viewModel.Socio = socio;
                Console.WriteLine($"Editando socio con ID: {socio.Id}");
            }
            else
            {
                Console.WriteLine("Creando un nuevo socio.");
            }
        }
    }
}
