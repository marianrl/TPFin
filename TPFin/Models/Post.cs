using System;
using System.Collections.Generic;
using System.Text;

namespace TPFin.Models
{
    public class Post
    {
        public int id { get; set; }
        public int idUser { get; set; }
        public Usuario user { get; set; }
        public string contenido { get; set; }
        public List<Comentario> comentarios { get; set; } = new List<Comentario>();
        public List<Reaccion> reacciones { get; set; } = new List<Reaccion>();
        public DateTime fecha { get; set; }
        public ICollection<Tag> Tag { get; } = new List<Tag>();
        public List<PostsTags> PostsTags { get; set; }

        public Post() { }

        public Post(int id,Usuario user, string contenido)
        {
            this.id =id ;
            this.user = user;
            this.contenido = contenido;
            reacciones = new List<Reaccion>();
            comentarios = new List<Comentario>();
            //tags = new List<Tag>();
            this.fecha = DateTime.Now;
        }

        public Post(Usuario user, string contenido)
        {
            this.id = id;
            this.user = user;
            this.contenido = contenido;
            reacciones = new List<Reaccion>();
            comentarios = new List<Comentario>();
            //tags = new List<Tag>();
            this.fecha = DateTime.Now;
        }

    }
}
