using System;
using System.Collections.Generic;

#nullable disable

namespace Entety
{
    public partial class UserToUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FollowingId { get; set; }

        public virtual User Following { get; set; }
        public virtual User User { get; set; }
    }
}
