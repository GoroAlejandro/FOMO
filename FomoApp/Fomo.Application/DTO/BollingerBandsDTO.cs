using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Application.DTO
{
    public class BollingerBandsDTO
    {
        public List<decimal> UpperBand {  get; set; } = new List<decimal>();

        public List<decimal> LowerBand { get; set; } = new List<decimal>();
    }
}
