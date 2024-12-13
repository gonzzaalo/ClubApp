using ClubApp.Class;
using ClubApp.View.Cuotas;
using ClubApp.View.Deportes;
using ClubApp.View.Deportistas;
using ClubApp.View.Profesores;
using ClubApp.View.Socios;
using CommunityToolkit.Mvvm.Messaging;

namespace ClubApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            //código para preparar la recepción de mensajes y la llamada al método RecibirMensaje
            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            //código para abrir la vista AddEditDeportistaView
            if (m.Value == "AbrirAddEditDeportistaView")
            {
                await Navigation.PushAsync(new AddEditDeportistaView(m.Deportista));
            }
            if (m.Value == "AbrirDeportistas")
            {
                await Navigation.PushAsync(new DeportistaView());
            }
            if (m.Value == "VolverADeportistas")
            {
                await Navigation.PopAsync();
            }
            //código para abrir la vista AddEditCuotaView

            if (m.Value == "AbrirAddEditCuotaView")
            {
                await Navigation.PushAsync(new AddEditCuotaView(m.Cuota));
            }
            if (m.Value == "AbrirCuotas")
            {
                await Navigation.PushAsync(new CuotaView());
            }
            if (m.Value == "VolverACuotas")
            {
                await Navigation.PopAsync();
            }
            //código para abrir la vista AddEditDeporteView
            if (m.Value == "AbrirAddEditDeporteView")
            {
                await Navigation.PushAsync(new AddEditDeporteView(m.Deporte));
            }
            if (m.Value == "AbrirDeportes")
            {
                await Navigation.PushAsync(new DeporteView());
            }
            if (m.Value == "VolverADeportes")
            {
                await Navigation.PopAsync();
            }
            //código para abrir la vista AddEditProfesorView
            if (m.Value == "AbrirAddEditProfesorView")
            {
                await Navigation.PushAsync(new AddEditProfesorView(m.Profesor));
            }
            if (m.Value == "AbrirProfesores")
            {
                await Navigation.PushAsync(new ProfesorView());
            }
            if (m.Value == "VolverAProfesores")
            {
                await Navigation.PopAsync();
            }
            //código para abrir la vista AddEditSocioView
            if (m.Value == "AbrirAddEditSocioView")
            {
                await Navigation.PushAsync(new AddEditSocioView(m.Socio));
            }
            if (m.Value == "AbrirSocios")
            {
                await Navigation.PushAsync(new SocioView());
            }
            if (m.Value == "VolverASocios")
            {
                await Navigation.PopAsync();
            }
        }
        //private void OnCounterClicked(object sender, EventArgs e)
        //{
        //    count++;

        //    if (count == 1)
        //        CounterBtn.Text = $"Clicked {count} time";
        //    else
        //        CounterBtn.Text = $"Clicked {count} times";

        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeportistaView());
        }

        private async void BtbCuotas(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CuotaView());
        }

        private async void BtnDeportes(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeporteView());
        }

        private async void BtnProfesor(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfesorView());
        }

        private async void BtnSocios(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SocioView());
        }
    }

}
