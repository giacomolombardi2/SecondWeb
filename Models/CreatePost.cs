using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Lombardi.Giacomo._5h.SecondaWeb.Models
{
    public class CreatePost
    {
        public IFormFile MyCsv { get;set;}
        public string Descrizione { get;set;}
    }
}