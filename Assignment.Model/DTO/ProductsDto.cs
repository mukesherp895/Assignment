namespace Assignment.Model.DTO
{
    public class ProductsDto : CommonListDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CompanyName { get; set; }
        public int CompanyInfoId { get; set; }
    }
}
