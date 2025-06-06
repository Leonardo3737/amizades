using Amizades.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Amizades.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [DisplayName("Nome Completo")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um email válido.")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [DisplayName("Senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome de usuario deve ter no mínimo 3 caracteres.")]
        [DisplayName("Nome de Usuário")]
        public string Username { get; set; }
        public List<Friendship>? SentFriendRequests { get; set; }
        public List<Friendship>? ReceivedFriendRequests { get; set; }
        public List<Publication>? Publications { get; set; }
        public List<Comment>? Comments { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int GetFriendsCount()
        {
            var friendsIds = new List<int>();

            SentFriendRequests?.ForEach(friend =>
            {
                if (friend.Status == FriendshipStatus.Accepted && !friendsIds.Contains(friend.Id))
                {
                    friendsIds.Add(friend.Id);
                }
            });

            ReceivedFriendRequests?.ForEach(friend =>
            {
                if (friend.Status == FriendshipStatus.Accepted && !friendsIds.Contains(friend.Id))
                {
                    friendsIds.Add(friend.Id);
                }
            });

            return friendsIds.Count;
        }

    }
}
