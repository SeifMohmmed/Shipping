namespace Shipping.Application.Abstraction.Branch.DTO;
public class BranchBaseDTO
{
    public required string Name { get; set; }

    public string? Location { get; set; }

    public DateTime BranchDate { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; }

    public int? RegionId { get; set; }

}

public class BranchToAddDTO : BranchBaseDTO
{

}

public class BranchToUpdateDTO : BranchBaseDTO
{

}

public class BranchDTO : BranchBaseDTO
{
    public int Id { get; set; }

    public string? RegionName { get; set; }

    public List<string> Users { get; set; } = [];
}