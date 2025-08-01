namespace Shipping.Application.Abstraction.User.DTO;
public record EmployeeDTO
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string BranchName { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

}