using Fomo.Domain.Entities;

namespace Fomo.Application.DTO.StockDataDTO
{
    public class StockPageDTO
    {
        public List<Stock> Data { get; set; } = new List<Stock>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
