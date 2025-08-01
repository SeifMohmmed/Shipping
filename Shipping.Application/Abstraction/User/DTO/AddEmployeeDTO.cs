namespace Shipping.Application.Abstraction.User.DTO;
public record AddEmployeeDTO
(string Email, string Password, string FullName, string PhoneNumber, string Address, int BranchId, int RegionID,
    string RoleName);