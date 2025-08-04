using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;

public class SpecialCityCost
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public decimal Price { get; set; }

    public bool IsDeleted { get; set; } = false;

    [ForeignKey(nameof(CitySetting))]
    public int CitySettingId { get; set; }

    public virtual CitySetting? CitySetting { get; set; }


    [ForeignKey(nameof(Merchant))]
    public string MerchantId { get; set; } = string.Empty;

    public virtual ApplicationUser? Merchant { get; set; }
}