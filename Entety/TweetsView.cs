using System;

#nullable disable

namespace Entety
{
    public partial class TweetsView
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime? CreateDate { get; set; }
        public int UserId { get; set; }
    }
}
