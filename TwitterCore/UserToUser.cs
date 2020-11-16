namespace TwitterCore
{
    class UserToUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FollowingId { get; set; }
        public UserToUser(int userId, int followingId)
        {
            this.UserId = userId;
            this.FollowingId = followingId;
        }
        
    }
}