using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FluentValidation;

namespace PruebaAtlantidaFE.Models
{

    public class MovimientoValidator : AbstractValidator<MovimientoCLS>
    {
        public MovimientoValidator()
        {
            RuleFor(x => x.Fecha).NotNull();
            RuleFor(x => x.Monto).NotEmpty().GreaterThan(0);
            RuleFor(x => x.IdtarjetaCredito).NotNull();
            RuleFor(x => x.IdTipoMovimiento).NotNull();
        }
    }

    public class MovimientoCLS
    {
        [Key]
        public int Id { get; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        public string Descripción { get; set; }

        [Required]
        [Range(0.00000000001, double.MaxValue, ErrorMessage = "El valor debe ser mayor que 0")]
        public double Monto { get; set; }

        [Required]
        [Column("IdtarjetaCredito")]
        public int IdtarjetaCredito { get; set; }

        [Required]
        [Column("IdTipoMovimiento")]
        public int IdTipoMovimiento { get; set; }
    }
}