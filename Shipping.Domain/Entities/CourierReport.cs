using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;

public class CourierReport
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    //----------- Obj From User and ForeignKey UserId ---------------------------------
    [ForeignKey(nameof(Courier))]
    public string CourierId { get; set; } = string.Empty;

    public virtual ApplicationUser? Courier { get; set; }

    //-----------  Order  --------------------------------
    [ForeignKey(nameof(Order))]
    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

}