using System.ComponentModel.DataAnnotations;

namespace Shipping.Domain.Entities;
public class TokenRequestModel
{
    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }

}