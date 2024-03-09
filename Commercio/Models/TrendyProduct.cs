namespace Commercio.Models
{
    public class TrendyProduct: Entity<Guid>
    {
        public int ProductId { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
    }
}
