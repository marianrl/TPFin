using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TPFin.Models;

namespace TPFin.Data
{
    public class MyContext : DbContext
    {
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Post> post { get; set; }
        public DbSet<Comentario> comentarios { get; set; }        
        public DbSet<Reaccion> reacciones { get; set; }
        public DbSet<Tag> tags { get; set; }

        public MyContext() { }
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=DESKTOP-KMQE4HH\INITIAL;initial catalog=TP_FINAL;trusted_connection=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //nombre de la tabla
            modelBuilder.Entity<Usuario>().ToTable("Usuarios").HasKey(u => u.id);
            modelBuilder.Entity<Post>().ToTable("Post").HasKey(p => p.id);
            modelBuilder.Entity<Comentario>().ToTable("Comentario").HasKey(c => c.id);
            modelBuilder.Entity<Reaccion>().ToTable("Reaccion").HasKey(r => r.id);
            modelBuilder.Entity<Tag>().ToTable("Tag").HasKey(k => k.id);
            modelBuilder.Entity<UsuarioAmigo>().ToTable("Usuario_Amigo").HasKey(k => new { k.idAmigo, k.idUser });
            modelBuilder.Entity<PostsTags>().ToTable("Posts_Tags").HasKey(k => new { k.idPost, k.idTag });

            //relaciones
            modelBuilder.Entity<Post>()
                .HasOne(P => P.user)
                .WithMany(U => U.misPosts)
                .HasForeignKey(P => P.idUser)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Comentario>()
                .HasOne(C => C.usuario)
                .WithMany(U => U.misComentarios)
                .HasForeignKey(C => C.idUser)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Comentario>()
                .HasOne(C => C.post)
                .WithMany(P => P.comentarios)
                .HasForeignKey(C => C.idPost)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaccion>()
                .HasOne(R => R.usuario)
                .WithMany(U => U.misReacciones)
                .HasForeignKey(R => R.idUser)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaccion>()
                .HasOne(R => R.post)
                .WithMany(P => P.reacciones)
                .HasForeignKey(R => R.idPost)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsuarioAmigo>()
               .HasOne(UA => UA.user)
               .WithMany(U => U.misAmigos)
               .HasForeignKey(u => u.idAmigo)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsuarioAmigo>()
                .HasOne(UA => UA.amigo)
                .WithMany(U => U.amigosMios)
                .HasForeignKey(u => u.idUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasMany(T => T.Tag)
                .WithMany(P => P.Post)
                .UsingEntity<PostsTags>(
                    etp => etp.HasOne(tp => tp.Tag).WithMany(p => p.PostsTags).HasForeignKey(t => t.idTag),
                    etp => etp.HasOne(tp => tp.Post).WithMany(p => p.PostsTags).HasForeignKey(t => t.idPost),
                    etp => etp.HasKey(k => new { k.idPost, k.idTag })
                );

            modelBuilder.Entity<Usuario>(
                usr => 
                {
                    usr.Property(u => u.dni).HasColumnType("int");
                    usr.Property(u => u.dni).IsRequired(true);
                    usr.Property(u => u.nombre).HasColumnType("varchar(200)");
                    usr.Property(u => u.apellido).HasColumnType("varchar(200)");
                    usr.Property(u => u.email).HasColumnType("varchar(200)");
                    usr.Property(u => u.password).HasColumnType("varchar(200)");
                    usr.Property(u => u.intentosFallidos).HasColumnType("int");
                    usr.Property(u => u.bloqueado).HasColumnType("bit");
                    usr.Property(u => u.isAdm).HasColumnType("bit");
                });

            //Datos de prueba
            modelBuilder.Entity<Usuario>().HasData(
                new {id = 1, dni = 111, nombre = "Mariano", apellido = "Rojas", email = "mariano@mail.com", password = "111", intentosFallidos = 0, bloqueado = false , isAdm = true});
            modelBuilder.Entity<Usuario>().HasData(
                new { id = 2, dni = 222, nombre = "Alan", apellido = "Carballal", email = "alan@mail.com", password = "222", intentosFallidos = 0, bloqueado = false, isAdm = false });
            modelBuilder.Entity<Usuario>().HasData(
                new { id = 3, dni = 222, nombre = "Manuel", apellido = "Fraga", email = "Manuel@mail.com", password = "333", intentosFallidos = 0, bloqueado = false, isAdm = false });
            modelBuilder.Entity<Post>().HasData(
                new { id = 1, idUser = 1, contenido = "Como estan?", fecha = DateTime.Now });
            modelBuilder.Entity<Post>().HasData(
                new { id = 2, idUser = 2, contenido = "Todo bien por suerte", fecha = DateTime.Now });
            modelBuilder.Entity<Post>().HasData(
                new { id = 3, idUser = 3, contenido = "Hola", fecha = DateTime.Now });
            modelBuilder.Entity<Comentario>().HasData(
                new { id = 1, idPost = 1, idUser= 1,  contenido = "Argentina", fecha = DateTime.Now });
            modelBuilder.Entity<Comentario>().HasData(
                new { id = 2, idPost = 2, idUser = 2, contenido = "Argentina", fecha = DateTime.Now });
            modelBuilder.Entity<Comentario>().HasData(
                new { id = 3, idPost = 3, idUser = 3, contenido = "Argentina", fecha = DateTime.Now });

            modelBuilder.Ignore<RedSocial>();
        }

        public DbSet<TPFin.Models.UsuarioAmigo>? UsuarioAmigo { get; set; }
    }
}
