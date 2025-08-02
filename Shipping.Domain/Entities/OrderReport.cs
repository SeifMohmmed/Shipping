using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;
public class OrderReport
{
    public int Id { get; set; }

    public string ReportDetails { get; set; } = string.Empty;

    public DateTime ReportDate { get; set; } = DateTime.Now;

    [Required, ForeignKey(nameof(Order))]
    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

}
