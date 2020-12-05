namespace Billboards.Models
{
    public interface IModel
    {
        long Id { get; set; }
        bool IsDeleted { get; set; }

    }
}
