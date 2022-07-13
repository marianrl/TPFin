using System;
using System.Collections.Generic;
using System.Text;

namespace TPFin.Models
{
    public class Comentario
    {
        public int id { get; set; }
        public int idPost { get; set; }
        public Post post { get; set; }
        public int idUser { get; set; }
        public Usuario usuario { get; set; }
        public string contenido { get; set; }
        public DateTime fecha { get; set; }        

        public Comentario() { }

        public Comentario(Post post, Usuario usuario, string contenido)
        {
            this.id = id;
            this.post = post;
            this.usuario = usuario;
            this.contenido = contenido;
            this.fecha = DateTime.Now;
        }

        public Comentario(int id, Post post, Usuario usuario, string contenido)
        {
            this.id = id;
            this.post = post;
            this.usuario = usuario;
            this.contenido = contenido;
            this.fecha = DateTime.Now;
        }

    }
}
