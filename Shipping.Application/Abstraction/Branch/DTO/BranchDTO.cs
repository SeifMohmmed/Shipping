using Shipping.Domain.Entities;

namespace Shipping.Application.Abstraction.Branch.DTO;
public class BranchDTO
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Location { get; set; }

    public DateTime BranchDate { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; }

    public int? RegionId { get; set; }

    public string? RegionName { get; set; }

    public List<ApplicationUser> UserName { get; set; } = [];

}

public class BranchToAddDTO
{
    public required string Name { get; set; }

    public string? Location { get; set; }

    public DateTime BranchDate { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; }

    public int? RegionId { get; set; }

}

public class BranchToUpdateDTO
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Location { get; set; }

    public DateTime BranchDate { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; }

    public int? RegionId { get; set; }

}