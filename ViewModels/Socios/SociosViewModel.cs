using ClubApp.Class;
using ClubApp.Models;
using ClubApp.Services;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.ViewModels.Socios
{
    public class SociosViewModel : NotificationObject
    {
        GenericService<Socio> socioService = new GenericService<Socio>();
        GenericService<Cuota> cuotaService = new GenericService<Cuota>();

        private bool activityStart;

        public bool ActivityStart
        {
            get { return activityStart; }
            set
            {
                activityStart = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Socio> socios;

        public ObservableCollection<Socio> Socios
        {
            get { return socios; }
            set
            {
                socios = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Cuota> cuotas;

        public ObservableCollection<Cuota> Cuotas
        {
            get { return cuotas; }
            set
            {
                cuotas = value;
                OnPropertyChanged();
            }
        }

        private Socio socioCurrent;

        public Socio SocioCurrent
        {
            get { return socioCurrent; }
            set
            {
                socioCurrent = value;
                OnPropertyChanged();
                EditarCommand.ChangeCanExecute();
                EliminarCommand.ChangeCanExecute();
            }
        }

        private bool isRefreshing;

        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public Command AgregarCommand { get; }
        public Command EditarCommand { get; }
        public Command EliminarCommand { get; }

        public SociosViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerSocios());
            Task.Run(async () => await ObtenerCuotas());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverASocios")
            {
                await RefreshDeportistas(this);
            }
        }

        private async Task RefreshDeportistas(object obj)
        {
            IsRefreshing = true;
            await ObtenerSocios();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return SocioCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert("Eliminar", $"¿Estás seguro de eliminar este socio {SocioCurrent.Nombre}?", "Si", "No");
            if (respuesta)
            {
                ActivityStart = true;
                await socioService.DeleteAsync(SocioCurrent.Id);
                await ObtenerSocios();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return SocioCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditSocioView") { Socio = SocioCurrent });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditSocioView"));
        }

        public async Task ObtenerSocios()
        {
            ActivityStart = true;
            var socios = await socioService.GetAllAsync();
            Socios = new ObservableCollection<Socio>(socios);
            ActivityStart = false;
        }

        public async Task ObtenerCuotas()
        {
            var cuotas = await cuotaService.GetAllAsync();  // O tu lógica de carga de cuotas
            Cuotas = new ObservableCollection<Cuota>(cuotas);
        }
    }
}
