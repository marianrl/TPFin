using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TPFin.Data;

namespace TPFin.Models
{
     public class RedSocial
     {
        private DbSet<Usuario> misUsuarios;
        private DbSet<Post> efPosts { get; set; }
        public List<Tag> tags { get; set; }
        public Usuario usuarioActual { get; set; }        
        private DbSet<Comentario> efComent { get; set; }
        private DbSet<Reaccion> efReacciones { get; set; }
        private DbSet<Tag> efTags { get; set; }
        private int intentos;

        private MyContext context;
        public RedSocial()
        {
            tags = new List<Tag>();
            intentos = 0;
            inicializarAtributos();
        }

        private void inicializarAtributos()
        {
            try
            {
                //creo contexto
                context = new MyContext();
                context.usuarios.Include(u => u.misAmigos).ThenInclude(ua => ua.user).Include(u => u.amigosMios).ThenInclude(ua => ua.amigo).Load();
                misUsuarios = context.usuarios;                
                context.post.Include(p => p.Tag).Load();
                context.tags.Include(t => t.Post).Load();
                efPosts = context.post;
                context.comentarios.Load();
                efComent = context.comentarios;
                context.reacciones.Load();
                efReacciones = context.reacciones;
                context.tags.Load();
                efTags = context.tags;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }       
        public void registrarUsuario(string nombre, string apellido, string mail, int dni, string pass, bool isAdm) // OK
        {
            Usuario nuevo = new Usuario(nombre,apellido, mail, dni, pass, isAdm);
            context.usuarios.Add(nuevo);
            context.SaveChanges();
            context.usuarios.Include(u => u.misAmigos).ThenInclude(ua => ua.user).Include(u => u.amigosMios).ThenInclude(ua => ua.amigo).Load();
        }
        public bool modificaUsuario(Usuario usuarioModificado) // OK
        {
            bool salida = false;
            foreach (Usuario u in context.usuarios)
                if (u.id == usuarioModificado.id)
                {
                    u.nombre = usuarioModificado.nombre;
                    u.apellido = usuarioModificado.apellido;
                    u.email = usuarioModificado.email;
                    u.dni = usuarioModificado.dni;
                    context.usuarios.Update(u);
                    salida = true;
                }
            if (salida)
                context.SaveChanges();
            return salida;
        }
        public bool eliminarUsuario(Usuario u) // OK
        {
            bool result = false;
            foreach (Usuario lU in context.usuarios)
            {
                if (lU.id == u.id)
                {
                    context.usuarios.Remove(lU);
                    result = true;
                }
            }
            if (result)
                context.SaveChanges();
            return result;
        }
        public bool iniciarSesion(string usuario, string pass)
        {

            List<Usuario> salida = new List<Usuario>();
            var query = from Usuario in context.usuarios
                        where Usuario.email == usuario && Usuario.password == pass
                        select Usuario;

            if (query.Count() != 0)
            {

                Usuario user = query.First();
                if (!user.bloqueado)
                {
                    usuarioActual = user;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                intentos++;
                return false;
            }
        }
        //public void cerrarSesion(Forms.Home home,Login frm)
        //{
        //    usuarioActual = null;
        //    home.Close();
        //    frm.Show();
        //}
        //public void cerrarSesionAdm(Forms.Admin adim, Login frm)
        //{
        //    usuarioActual = null;
        //    adim.Close();
        //    frm.Show();
        //}
        public void agregarAmigo(Usuario amigo)
        {

            UsuarioAmigo am1 = new UsuarioAmigo(context.usuarios.Where(u => u.id == usuarioActual.id).FirstOrDefault(), context.usuarios.Where(u => u.id == amigo.id).FirstOrDefault());
            UsuarioAmigo am3 = new UsuarioAmigo(context.usuarios.Where(u => u.id == amigo.id).FirstOrDefault(), context.usuarios.Where(u => u.id == usuarioActual.id).FirstOrDefault());
            
            context.usuarios.Where(u => u.id == usuarioActual.id).FirstOrDefault().misAmigos.Add(am1);
            context.usuarios.Where(u => u.id == amigo.id).FirstOrDefault().misAmigos.Add(am3);

            context.usuarios.Update(context.usuarios.Where(u => u.id == usuarioActual.id).FirstOrDefault());
            context.usuarios.Update(context.usuarios.Where(u => u.id == amigo.id).FirstOrDefault());
            context.SaveChanges();

        }
        public void quitarAmigo(Usuario exAmigo)
        {
            UsuarioAmigo am1 = context.usuarios.Where(u => u.id == usuarioActual.id).FirstOrDefault().misAmigos.Where(a => a.idUser == exAmigo.id).FirstOrDefault();
            UsuarioAmigo am3 = context.usuarios.Where(u => u.id == exAmigo.id).FirstOrDefault().misAmigos.Where(a => a.idUser == usuarioActual.id).FirstOrDefault();
            
            context.usuarios.Where(u => u.id == usuarioActual.id).FirstOrDefault().misAmigos.Remove(am1);
            context.usuarios.Where(u => u.id == exAmigo.id).FirstOrDefault().misAmigos.Remove(am3);

            context.usuarios.Update(context.usuarios.Where(u => u.id == usuarioActual.id).FirstOrDefault());
            context.usuarios.Update(context.usuarios.Where(u => u.id == exAmigo.id).FirstOrDefault());
            context.SaveChanges();
        }
        public bool efPostear(Usuario u, string contenido, List<Tag> newTags)  //OK 
        {
            try
            {
                Usuario user = context.usuarios.Where(usr => usr.id == u.id).FirstOrDefault();

                if (user != null)
                {
                    Post nPost = new Post(u, contenido);
                    user.misPosts.Add(nPost);
                    efPosts.Add(nPost);
                    foreach (Tag t in newTags)
                    {
                        Tag pT = context.tags.Where(tg => tg.palabra == t.palabra).FirstOrDefault();
                        if (pT == null)
                        {
                            efTags.Add(t);
                        }                        
                    }
                    context.SaveChanges();
                    nPost = context.post.OrderByDescending(x => x.id).First();

                    foreach (Tag t in newTags)
                    {
                        Tag lT = context.tags.Where(st => st.palabra == t.palabra).FirstOrDefault();
                        nPost.Tag.Add(lT);
                        context.post.Update(nPost);
                        context.SaveChanges();
                    }

                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool modificarPost(Post p) // OK
        {
            bool salida = false;
            foreach(Post efp in efPosts)
            {
                if(efp.id == p.id)
                {
                    efp.contenido = p.contenido;
                    context.post.Update(efp);
                    salida = true;
                }
            }
            if(salida)
                context.SaveChanges();
            return salida;
        }
        public bool eliminarPost(Post p) // OK
        {
            bool salida = false;
            foreach(Post efp in efPosts)
            {
                if (efp.id == p.id)
                {
                    efp.contenido = p.contenido;
                    context.post.Remove(efp);
                    salida = true;
                }
            }
            if (salida)
                context.SaveChanges();
            return salida;
        }
        public bool comentar(Post p, Comentario c) // OK
        {
            try
            {
                Post post = context.post.Where(pos => pos.id == p.id).FirstOrDefault();

                if (post != null)
                {
                    post.comentarios.Add(c);
                    context.comentarios.Add(c);
                    context.post.Update(post);
                    context.SaveChanges();
                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void modificarComentario(Comentario c) // OK
        {
            context.comentarios.Update(c);
            context.SaveChanges();
        }
        public void modificarPostAdmin(int postId, string nuevoContenido) 
        {
            if (postId != 0)
            {
                Post p = searchPost(postId);
                p.contenido = nuevoContenido;
                context.post.Update(p);
                context.SaveChanges();
            }
        }
        public void comentarAdmin(Post p, Comentario c)
        {
            Post post = context.post.Where(pos => pos.id == p.id).FirstOrDefault();

            post.comentarios.Add(c);
            context.comentarios.Add(c);
            context.post.Update(post);
            context.SaveChanges();
        }
        public void modificarCommentAdmin(Post p,int comentId, string nuevoContenido) 
        {
            if (comentId != 0)
            {
                Comentario c = searchComent(comentId);
                c.contenido = nuevoContenido;
                context.comentarios.Update(c);
                context.SaveChanges();
            }
        }
        public void quitarComentario(Comentario c) // OK
        {
            context.comentarios.Remove(c);
            context.SaveChanges();
        }
        public void reaccionar(Post p, Reaccion r) // OK
        {
            
            bool newReaction = true;
            foreach (Reaccion reaccion in p.reacciones)
            {
                if (reaccion.usuario.id == usuarioActual.id)
                {
                    newReaction = false;
                    modificarReaccion(p, r);
                }
            }
            if (newReaction)
            {
                context.reacciones.Add(r);
                context.SaveChanges();                                 
            }
        }        
        public void modificarReaccion(Post p, Reaccion r)
        {
            foreach (Reaccion reaccion in p.reacciones)
            {
                
                if (reaccion.usuario.id == usuarioActual.id)
                {
                    reaccion.tipoReaccion = r.tipoReaccion;
                    context.reacciones.Update(reaccion);
                    context.SaveChanges();                    
                }
            }
        }
        public void quitarReaccion(Post p, Reaccion r)
        {
            context.reacciones.Remove(r);
            context.SaveChanges();            
        }
        public Usuario mostrarDatos(Usuario u)
        {
          return u;
        }
        public List<Post> mostrarPosts()
        {
          return usuarioActual.misPosts;
        }
        public List<Post> mostrarPostsAmigos()
        {
            List<Post> postsAmigos = new List<Post>();

            foreach (UsuarioAmigo user in usuarioActual.misAmigos)
            {
                postsAmigos.AddRange(user.amigo.misPosts);
            }

            return postsAmigos;
        }
        public List<Post> buscarPosts(String pContenido, DateTime fechaDesde,DateTime fechaHasta, List<string> bTags)
        {
            List<Post> p = new List<Post>();
            List<Post> partialList = new List<Post>();
            string fDesde = fechaDesde.Date.ToString("dd/MM/yyyy");
            string hDesde = fechaHasta.Date.ToString("dd/MM/yyyy");
            List<Tag> pTag = new List<Tag>();

            if (bTags != null && bTags.Count > 0)
            {                
                //filtrado inicial por tags
                foreach (string palabra in bTags)
                {
                    Tag t = context.tags.Where(t => t.palabra == palabra).FirstOrDefault();
                    if (t != null)
                    {
                        foreach (Post pp in t.Post)
                        {
                            partialList.Add(pp);
                            p.Add(pp);
                        }
                    }
                }
                if (pContenido != "" && p.Count > 0)
                {
                    foreach (Post p1 in partialList)
                    {
                        if (p1.contenido != pContenido)
                        {
                            p.Remove(p1);
                        }
                    }

                    foreach (Post p1 in partialList)
                    {
                        if (p1.fecha.Date < fechaDesde.Date && p1.fecha.Date > fechaHasta.Date)
                        {
                            if (p.Contains(p1))
                            {
                                p.Remove(p1);
                            }                            
                        }
                    }
                }
                else
                {
                    //si no tiene contenido
                    foreach (Post p1 in partialList)
                    {
                        if (p1.fecha.Date < fechaDesde.Date && p1.fecha.Date > fechaHasta.Date)
                        {
                            if (p.Contains(p1))
                            {
                                p.Remove(p1);
                            }
                        }
                    }
                }
            }
            else
            {
                //si no hay tag
                partialList = context.post.Where(t => t.contenido == pContenido).ToList();

                foreach (Post pp in partialList)
                {
                    p.Add(pp);
                }

                if (pContenido != "" && p.Count > 0)
                {
                    foreach (Post p1 in partialList)
                    {
                        if (p1.contenido != pContenido)
                        {
                            p.Remove(p1);
                        }
                    }

                    foreach (Post p1 in partialList)
                    {
                        if (p1.fecha.Date < fechaDesde.Date && p1.fecha.Date > fechaHasta.Date)
                        {
                            if (p.Contains(p1))
                            {
                                p.Remove(p1);
                            }
                        }
                    }
                }
                else
                {
                    //si no tiene contenido ni tags
                    partialList = context.post.ToList();
                    
                    foreach (Post pp in partialList)
                    {
                        p.Add(pp);
                    }
                    
                    foreach (Post p1 in partialList)
                    {
                        if (p1.fecha.Date < fechaDesde.Date && p1.fecha.Date > fechaHasta.Date)
                        {
                            if (p.Contains(p1))
                            {
                                p.Remove(p1);
                            }
                        }
                    }
                }
            }

            return p;
        }
        public Comentario searchComent(int id)
        {

            Comentario c = context.comentarios.Where(pComment => pComment.id == id).FirstOrDefault();
            return c;
        }
        public Post searchPost(int idPost)
        {
            Post pRetornar = context.post.Where(p => p.id == idPost).FirstOrDefault();
            return pRetornar;
        }
        public Usuario searchUser(int idUser)
        {
            return context.usuarios.Where(u => u.id == idUser).FirstOrDefault();
        }
        public Tag searchTag(int idTag)
        {
            Tag t = context.tags.Where(pTag => pTag.id == idTag).FirstOrDefault();
            return t;
        }
        public bool bloqUser(int IdUsuario, bool Bloqueado)
        {
            bool salida = false;
            Usuario u = context.usuarios.Where(usr => usr.id == IdUsuario).FirstOrDefault();
            u.bloqueado = Bloqueado;
            u.intentosFallidos = 3;
            context.usuarios.Update(u);
            salida = true;
  
            if (salida)
                context.SaveChanges();
            return salida;
        }
        public void eliminarTag (int tagId)
        {
            Tag t = searchTag(tagId);
            context.tags.Remove(t);
            context.SaveChanges();
        }        
        public DbSet<Usuario> getAllUsers()
        {
            return context.usuarios;
        }        
        public Usuario getUserByUserName(string userName)
        {
            Usuario u = context.usuarios.Where(usr => usr.email == userName).FirstOrDefault();
            return u;
        }
        public List<List<string>> obtenerUsuarios() //LINQ DE USUARIO (FALTA)
        {
            List<List<string>> salida = new List<List<string>>();
            foreach (Usuario u in context.usuarios)
                salida.Add(new List<string> { u.id.ToString(), u.nombre + " " + u.apellido });
            return salida;
        }
        public List<Post> obtenerPosts() //LINQ DE POSTS (FALTA)
        {
            List<Post> salida = new List<Post>();
            foreach (Post p in context.post)
                salida.Add(p);
            return salida;
        }
        public Comentario obtenerEfComments(int cId) 
        {
            
            return context.comentarios.Where(c => c.id == cId).FirstOrDefault();
        }
        public Reaccion obtenerEfReaccion(int uId)
        {
            return context.reacciones.Where(c => c.idUser == uId).FirstOrDefault();
        }
        public List<Comentario> obtenerComentario() //LINQ DE COMENTARIO (FALTA)
        {
            List<Comentario> salida = new List<Comentario>();
            foreach (Comentario c in context.comentarios)
                salida.Add(c);
            return salida;
        }
        public DbSet<Tag> obtenerEfTags()
        {
            return context.tags;
        }
        public void cerrarContextP()
        {
            context.Dispose();
        }

    }
}
