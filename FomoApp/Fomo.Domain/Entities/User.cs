using System.ComponentModel.DataAnnotations;

namespace Fomo.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(320)]
        public string Email { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public bool SmaAlert { get; set; } = false;
        public bool BollingerAlert { get; set; } = false;
        public bool StochasticAlert { get; set; } = false;
        public bool RsiAlert { get; set; } = false;

        public List<TradeResult>? TradeResults { get; set; }
    }
}
