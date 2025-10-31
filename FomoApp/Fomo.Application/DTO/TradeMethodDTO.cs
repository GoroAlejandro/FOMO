using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Application.DTO
{
    public class TradeMethodDTO
    {
        public bool Sma { get; set; } = false;
        public bool Bollinger { get; set; } = false;
        public bool Stochastic { get; set; } = false;
        public bool Rsi { get; set; } = false;
        public bool Other { get; set; } = false;
    }
}
