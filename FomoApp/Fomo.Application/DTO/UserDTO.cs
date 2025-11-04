using Fomo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fomo.Application.DTO
{
    public class UserDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool? SmaAlert { get; set; }
        public bool? BollingerAlert { get; set; }
        public bool? StochasticAlert { get; set; }
        public bool? RsiAlert { get; set; }
        public List<TradeResultDTO>? TradeResults { get; set; }
    }
}
