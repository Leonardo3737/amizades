using System.ComponentModel;

namespace Amizades.ViewModels
{
    public class HomeUserViewModel
    {
        public int Id { get; set; }

        [DisplayName("Nome Completo")]
        public string Name { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Senha")]
        public string Password { get; set; }

        [DisplayName("Nome de Usuário")]
        public string Username { get; set; }

        public PaginationViewModel<FriendshipViewModel> ReceivedFriendRequests { get; set; }

        public PaginationViewModel<FriendshipViewModel> Friends { get; set; }
    }
}
