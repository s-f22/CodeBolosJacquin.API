using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeBolosJacquin.API.Domains;

public partial class BoloImagen
{
    [Key]
    public int Id { get; set; }

    public int BoloId { get; set; }

    [StringLength(255)]
    public string CaminhoImagem { get; set; } = null!;

    [ForeignKey("BoloId")]
    [InverseProperty("BoloImagens")]
    public virtual Bolo Bolo { get; set; } = null!;
}
