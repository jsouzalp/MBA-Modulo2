namespace FinPlanner360.Business.Models;

public class Entity
{
    public Guid UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? RemovedDate { get; set; }
}