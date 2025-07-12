using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;

public class CourierReport
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    //-----------  Order  --------------------------------
    [ForeignKey(nameof(Order))]
    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

}