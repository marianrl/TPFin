using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPFin.Models
{
    public class  UsuarioAmigo
    {
        public int idUser { get; set; }
        public Usuario user { get; set; }
        public int idAmigo { get; set; }
        public Usuario amigo { get; set; }

        public UsuarioAmigo() { }

        public UsuarioAmigo(Usuario ppal, Usuario segundo)
        {
            user = ppal;
            amigo = segundo;
        }
    }
}
