using System;
using System.ComponentModel.DataAnnotations;

namespace Lombardi.Giacomo._5h.SecondaWeb.Models
{
    public class Persona
    {
        public int PersonaId { get; set; }
        
        public string Nome { get; set; }

        public string Cognome { get; set; }
        
        public DateTime Data { get; set; }

        
    }
}