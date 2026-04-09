namespace DataAccess.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
        string? Status { get; set; }
    }
}
