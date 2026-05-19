using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeBolosJacquin.API.Domains;

public partial class Bolo
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(300)]
    [Unicode(false)]
    public string? Descricao { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Preco { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Peso { get; set; }

    [InverseProperty("Bolo")]
    public virtual ICollection<BoloImagen> BoloImagens { get; set; } = new List<BoloImagen>();

    [ForeignKey("BoloId")]
    [InverseProperty("Bolos")]
    public virtual ICollection<Categoria> Categoria { get; set; } = new List<Categoria>();
}
