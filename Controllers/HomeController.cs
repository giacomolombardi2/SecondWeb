using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lombardi.Giacomo._5h.SecondaWeb.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;

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
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Elenco()
        {
             var db = new PrenotazioneContext();
            return View(db.Prenotazioni);
        }


        [Authorize]
        [HttpGet]
        public IActionResult Prenota()
        {
            return View();
        }
        [HttpPost]
         public IActionResult Prenota(Prenotazione p)
        {
            var db = new PrenotazioneContext();
                db.Prenotazioni.Add(p);
                db.SaveChanges();
            return View("Elenco",db.Prenotazioni);
        }
        public IActionResult Cancella(int Id)
        {

            var db = new PrenotazioneContext();
            Prenotazione prenotazione=db.Prenotazioni.Find(Id);
                db.Prenotazioni.Remove(prenotazione);
                db.SaveChanges();
            return View("Elenco",db.Prenotazioni);
        }


        [Authorize]
        public IActionResult Modifica(int Id)
        {
            var db = new PrenotazioneContext();
            Prenotazione prenotazione=db.Prenotazioni.Find(Id);
            return View("Modifica",prenotazione);
        
        }

        [HttpPost]
        //public IActionResult Modifica(int id, [Bind("Nome,Email")] Prenotazione nuovo)
        //{
        public IActionResult Modifica(int id,Prenotazione nuovo)
        {
            var db = new PrenotazioneContext();
            var vecchio=db.Prenotazioni.Find(id);
            if(vecchio!=null){
                    vecchio.Nome=nuovo.Nome;
                    vecchio.Email=nuovo.Email;
                db.Prenotazioni.Update(vecchio);
                db.SaveChanges();
                return View("Elenco",db.Prenotazioni);
            }
            return NotFound();
        }
        public IActionResult Upload(){
            return View("Upload");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Upload(CreatePost c){
            MemoryStream stream =new MemoryStream();
            c.MyCsv.CopyTo(stream);
            stream.Seek(0,0);
            StreamReader fn=new StreamReader(stream);

            if(!fn.EndOfStream)
            {
                PrenotazioneContext db =new PrenotazioneContext();
                string riga=fn.ReadLine();
                while(!fn.EndOfStream){

                    riga=fn.ReadLine();
                    string[] colonne=riga.Split(';');
                   
                    Prenotazione p =new Prenotazione{Nome=colonne[0],Email=colonne[1]};
                    db.Prenotazioni.Add(p);
                }
                db.SaveChanges();
                return View("Elenco",db.Prenotazioni);
            }
            return View();
        }
    }
}

