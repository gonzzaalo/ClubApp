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

namespace ClubApp.ViewModels.Profesores
{
    public class ProfesoresViewModel : NotificationObject
    {
        GenericService<Profesor> profesorService = new GenericService<Profesor>();
        GenericService<Deporte> deporteService = new GenericService<Deporte>();

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
        private ObservableCollection<Profesor> profesores;
        public ObservableCollection<Profesor> Profesores
        {
            get { return profesores; }
            set
            {
                profesores = value;
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

        private Profesor profesorCurrent;
        public Profesor ProfesorCurrent
        {
            get { return profesorCurrent; }
            set
            {
                profesorCurrent = value;
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

        public ProfesoresViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerProfesores());
            Task.Run(async () => await ObtenerDeportes());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }
        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverAProfesores")
            {
                await RefreshProfesores(this);
            }
        }

        private async Task RefreshProfesores(object obj)
        {
            IsRefreshing = true;
            await ObtenerProfesores();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return ProfesorCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert("Eliminar", $"¿Estás seguro de eliminar este profesor {ProfesorCurrent.Nombre}?", "Si", "No");
            if (respuesta)
            {
                ActivityStart = true;
                await profesorService.DeleteAsync(ProfesorCurrent.Id);
                await ObtenerProfesores();

            }
        }

        private bool PermitirEditar(object arg)
        {
            return ProfesorCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditProfesorView") { Profesor = ProfesorCurrent });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditProfesorView"));
        }

        public async Task ObtenerProfesores()
        {
            ActivityStart = true;
            var profesores = await profesorService.GetAllAsync();
            Profesores = new ObservableCollection<Profesor>(profesores);
            ActivityStart = false;
        }

        public async Task ObtenerDeportes()
        {
            
            var deportes = await deporteService.GetAllAsync();
            Deporte = new ObservableCollection<Deporte>(deportes);
            
        }
    }
}
