using ClubApp.Models;
using ClubApp.ViewModels.Profesores;

namespace ClubApp.View.Profesores;

public partial class AddEditProfesorView : ContentPage
{
	private AddEditProfesorViewModel viewModel;
    public AddEditProfesorView()
	{
		InitializeComponent();
        viewModel = new AddEditProfesorViewModel();
        BindingContext = viewModel;
    }

    public AddEditProfesorView(Profesor profesor)
    {
        InitializeComponent();
        viewModel = new AddEditProfesorViewModel();
        BindingContext = viewModel;

        if (profesor != null)
        {
            viewModel.Profesor = profesor;
        }
    }
}