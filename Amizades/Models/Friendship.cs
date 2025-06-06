using Amizades.Models.Enums;

namespace Amizades.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        // O usuário que iniciou a solicitação
        public int RequesterId { get; set; }
        public User Requester { get; set; }

        // O usuário que recebeu a solicitação
        public int AddresseeId { get; set; }
        public User Addressee { get; set; }

        // Status da amizade: pendente, aceita, recusada etc.
        public FriendshipStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
    }
}
