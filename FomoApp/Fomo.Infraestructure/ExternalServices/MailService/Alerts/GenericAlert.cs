using Fomo.Domain.Entities;
using Fomo.Infrastructure.Repositories;

namespace Fomo.Infrastructure.ExternalServices.MailService.Alerts
{
    public class GenericAlert : IGenericAlert
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        
        public GenericAlert (IEmailService emailService, IUserRepository userRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
        }
        
        public async Task SendAlertAsync(AlertType alertType, string stock, string indicator)
        {
            var usersList = await _userRepository.GetUsersByAlertAsync(alertType);

            string subject = "New Stock Alert";

            if (usersList != null)
            {
                foreach (var user in usersList)
                {
                    string body = $"Hello {user.Name}!!\nA user achieved a positive outcome for the {stock} stock by using {indicator}";
                    await _emailService.SendAsync(user.Email, subject, body);
                }
            }
        }
    }
}
