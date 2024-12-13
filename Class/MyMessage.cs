using CommunityToolkit.Mvvm.Messaging.Messages;
using ClubApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubApp.Class
{

        public class MyMessage : ValueChangedMessage<string>
        {
            public Deportista Deportista { get; set; }
            public Cuota Cuota { get; set; }

            public Deporte Deporte { get; set; }

            public Profesor Profesor { get; set; }

            public Socio Socio { get; set; }
            public Usuario Usuario { get; set; }

            public MyMessage(string value) : base(value)
            {

            }
        }
    
}
