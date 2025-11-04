using Fomo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Application.DTO
{
    public class TradeResultDTO
    {
        public int? TradeResultId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public decimal EntryPrice { get; set; }
        public decimal ExitPrice { get; set; }
        public decimal Profit { get; set; }
        public int NumberOfStocks { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public TradeMethodDTO? TradeMethod { get; set; }
        public string? UserName { get; set; }
        public int? UserId { get; set; }
    }
}
