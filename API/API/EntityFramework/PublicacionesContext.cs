using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class PublicacionesContext : DbContext
{
    public PublicacionesContext()
    {
    }

    public PublicacionesContext(DbContextOptions<PublicacionesContext> options)
        : base(options)
    {
    }

    public DbSet<Publicacion> Publicaciones { get; set; }
    public DbSet<Imagen> Imagenes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Publicacion>()
       .HasMany(p => p.Imagenes) 
       .WithOne(i => i.Publicacion) 
       .HasForeignKey(i => i.PublicacionId);


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
