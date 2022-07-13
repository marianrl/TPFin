using System;
using System.Collections.Generic;
using System.Text;

namespace TPFin.Models
{
    public class Reaccion
    {
        public int id { get; set; }
        public string tipoReaccion { get; set; }
        public int idPost { get; set; }
        public Post post { get;  set; }
        public int idUser { get; set; }
        public Usuario usuario { get;  set; }

        public const string ME_GUSTA = "Me gusta";
        public const string NO_ME_GUSTA = "No me gusta";

        public Reaccion() { }
        public Reaccion(int id, string tipoReaccion, Post post, Usuario user)
        {
            this.id = id;
            this.tipoReaccion = tipoReaccion;
            this.post = post;
            this.usuario = user;

        }
        public Reaccion(string tipoReaccion, Post post, Usuario user)
        {
          
            this.tipoReaccion = tipoReaccion;
            this.post = post;
            this.usuario = user;

        }

    }
}
