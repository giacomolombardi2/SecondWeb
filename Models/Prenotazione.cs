using System;
using System.ComponentModel.DataAnnotations;

namespace Lombardi.Giacomo._5h.SecondaWeb.Models
{
public class Prenotazione {
    public int PrenotazioneId { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
    public bool? Partecipazione { get; set; }
    public DateTime DataPrenotazione{ get; set; }=DateTime.Now;
}

}
