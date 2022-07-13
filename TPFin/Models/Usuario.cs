using System;
using System.Collections.Generic;
using System.Text;

namespace TPFin.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public int dni { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int intentosFallidos { get; set; }
        public bool bloqueado { get; set; }
        public bool isAdm { get; set; }
        public virtual ICollection<UsuarioAmigo> misAmigos { get; set; } 
        public virtual ICollection<UsuarioAmigo> amigosMios { get; set; }
        public List<Post> misPosts { get; } = new List<Post>();
        public List<Comentario> misComentarios { get; set; }
        public List<Reaccion> misReacciones { get; set; }

        public Usuario() { }

        public Usuario(string nombre, string apellido, string mail, int dni, string pass, bool isAdm)
        {
            this.nombre = nombre;
            this.password = pass;
            this.apellido = apellido;
            this.email = mail;
            this.dni = dni;            
            this.intentosFallidos = 0;
            this.isAdm = isAdm;
            this.bloqueado = false;
            misPosts = new List<Post>();
            misComentarios = new List<Comentario>();
            
        }
        public Usuario(int id, string nombre, string apellido, string mail, int dni, string pass, int intentosFallidos, bool bloqueado, bool isAdm)
        {
            this.nombre = nombre;
            this.password = pass;
            this.apellido = apellido; 
            this.email = mail;  
            this.dni = dni;
            this.id = id;
            this.intentosFallidos = 0;
            this.isAdm = isAdm;
            this.bloqueado = bloqueado;
            misPosts = new List<Post>();
            misComentarios = new List<Comentario>();
        }

        public Usuario(int id, string nombre, string apellido, string mail, int dni, string pass)
        {
            this.nombre = nombre;
            this.password = pass;
            this.apellido = apellido;
            this.email = mail;
            this.dni = dni;
            this.id = id;
            intentosFallidos = 0;
            isAdm = false;
            bloqueado = false;
            misPosts = new List<Post>();
        }
    }
}
