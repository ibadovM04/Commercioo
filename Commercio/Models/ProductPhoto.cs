namespace Commercio.Models
{
    public class ProductPhoto:Entity<int>
    {
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; }

        public bool IaMain { get; set; }


        public Product Product { get; set; }
    }
}
