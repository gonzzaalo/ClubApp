using ClubApp.Class;
using ClubApp.Models;
using ClubApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.ViewModels.Profesores
{
    public class AddEditProfesorViewModel : ObservableObject
    {
        private readonly GenericService<Profesor> profesorService = new GenericService<Profesor>();

        private Profesor profesor;
        public Profesor Profesor
        {
            get => profesor;
            set
            {
                profesor = value ?? new Profesor();
                OnPropertyChanged();
            }
        }

        public AddEditProfesorViewModel()
        {
            Profesor = new Profesor();
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }
        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        private async Task Guardar()
        {
            if (Profesor == null)
            {
                Profesor = new Profesor();
            }
            try
            {
                if (Profesor.Id == 0)
                {
                    await profesorService.AddAsync(Profesor);
                }
                else
                {
                    await profesorService.UpdateAsync(Profesor);
                }

                WeakReferenceMessenger.Default.Send(new MyMessage("VolverAProfesores"));
            }
            catch (Exception ex)
            {
                // Manejo de errores, puedes también mostrar un mensaje al usuario si es necesario
                Console.WriteLine($"Error al guardar Profesor: {ex.Message}");
            }
        }
        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverADeportistas"));
        }
    }
}
