namespace Commercio.Models
{
    public class ProductOption:Entity<int>
    {
        public Guid ProductId { get; set; }
        public int OptionId { get; set; }
        

    }
}
