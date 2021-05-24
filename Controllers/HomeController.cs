using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lombardi.Giacomo._5h.SecondaWeb.Models;

namespace Lombardi.Giacomo._5h.SecondaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        [HttpGet]
         public IActionResult Prenota()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
         public IActionResult Elenco()
        {
            var db = new DBContext();
            return View(db);
        }
        [Authorize]
        [HttpPost]
      public IActionResult Prenota(Prenotazione p)
        {
            //tipo - etichetta - operatore - valore - terminatore di istruzione 
            //var a=10;

            //tipo - etichetta - operatore - valore - terminatore di istruzione 
            DBContext db = new DBContext();
            db.Prenotazioni.Add(p);
            db.SaveChanges();
            
            return View("Elenco", db);
        }

        [HttpGet]
         public IActionResult Modifica(int Id)
        {
            var db = new DBContext();
            Prenotazione prenotazione=db.Prenotazioni.Find(Id);
            return View("Modifica",prenotazione);
        
        }

         [HttpPost]
        
        public IActionResult Modifica(int id,Prenotazione nuovo)
        {
            var db = new DBContext();
            var vecchio=db.Prenotazioni.Find(id);
            if(vecchio!=null)
            {
                vecchio.Nome=nuovo.Nome;
                vecchio.Email=nuovo.Email;
                db.Prenotazioni.Update(vecchio);
                db.SaveChanges();
                return View("Elenco",db);
            }
            return NotFound();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Cancella( int id)
        {
            var db = new DBContext();
            Prenotazione prenotazione=db.Prenotazioni.Find(id);
            db.Remove(prenotazione);
            db.SaveChanges();
            return View("Cancella",db);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Upload()
        {
            return View();
        } 
         [Authorize]
         [HttpPost]
         public IActionResult Upload(CreatePost post)
        {
            MemoryStream memStream=new MemoryStream();
            post.MyCSV.CopyTo(memStream);
            //mette a zero il puntatore dello StreamReader
            memStream.Seek(0,0);

            StreamReader fim=new StreamReader(memStream);
            if(!fim.EndOfStream)
            {
                //accedi al database
                var db=new DBContext(); //oppure PrenotazioneContext db=new PrenotazioneContext(); 
                string riga = fim.ReadLine();
                while(!fim.EndOfStream)
                {
                    riga = fim.ReadLine();
                    string[] colonne = riga.Split(";");
                    Prenotazione p= new Prenotazione{Nome=colonne[0], Email=colonne[1], DataPrenotazione=Convert.ToDateTime(colonne[2])};
                    
                    db.Prenotazioni.Add(p);
                }                
                db.SaveChanges();
            
                return View("Elenco", db);                
            }         
            return View();
        }
    }
}