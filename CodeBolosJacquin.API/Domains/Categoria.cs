using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeBolosJacquin.API.Domains;

[Index("Nome", Name = "UQ__Categori__7D8FE3B2925BF922", IsUnique = true)]
public partial class Categoria
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Nome { get; set; } = null!;

    [ForeignKey("CategoriaId")]
    [InverseProperty("Categoria")]
    public virtual ICollection<Bolo> Bolos { get; set; } = new List<Bolo>();
}
