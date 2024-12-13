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

namespace ClubApp.ViewModels.Deportistas
{
    public class DeportistasViewModel : NotificationObject

    {
        GenericService<Deportista> deportistaService = new GenericService<Deportista>();
        GenericService<Deporte> deporteService = new GenericService<Deporte>();
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

        private ObservableCollection<Deportista> deportistas;

        public ObservableCollection<Deportista> Deportistas
        {
            get { return deportistas; }
            set
            {
                deportistas = value;
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
        private ObservableCollection<Deporte> deporte;
        public ObservableCollection<Deporte> Deporte
        {
            get { return deporte; }
            set
            {
                deporte = value;
                OnPropertyChanged();
            }
        }

        private Deportista deportistaCurrent;

        public Deportista DeportistaCurrent
        {
            get { return deportistaCurrent; }
            set
            {
                deportistaCurrent = value;
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

        public DeportistasViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerDeportistas());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverADeportistas")
            {
                await RefreshDeportistas(this);
            }
        }

        private async Task RefreshDeportistas(object obj)
        {
            IsRefreshing = true;
            await ObtenerDeportistas();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return DeportistaCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert("Eliminar", $"¿Estás seguro de eliminar este deportista {DeportistaCurrent.Nombre}?", "Si", "No");
            if (respuesta)
            {
                ActivityStart = true;
                await deportistaService.DeleteAsync(DeportistaCurrent.Id);
                await ObtenerDeportistas();

            }
        }

        private bool PermitirEditar(object arg)
        {
            return DeportistaCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditDeportistaView") { Deportista = DeportistaCurrent });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditDeportistaView"));
        }

        public async Task ObtenerDeportistas()
        {
            ActivityStart = true;
            var deportistas = await deportistaService.GetAllAsync();
            Deportistas = new ObservableCollection<Deportista>(deportistas);
            ActivityStart = false;

        }
        public async Task ObtenerCuotas()
        {
            var cuotas = await cuotaService.GetAllAsync();  // O tu lógica de carga de cuotas
            Cuotas = new ObservableCollection<Cuota>(cuotas);
        }
        public async Task ObtenerDeportes()
        {
            var deportes = await deporteService.GetAllAsync();  // O tu lógica de carga de deportes
            Deporte = new ObservableCollection<Deporte>(deportes);
        }
    }
}
