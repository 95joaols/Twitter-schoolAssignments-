namespace TwitterCore
{
    public class PrivateMessage
    {
        public int Id { get; set; }
        public int UserFromId { get; set; }
        public int UserToId { get; set; }
        public string Message { get; set; }
        public PrivateMessage(int userFromId, int userToId, string message)
        {
            UserFromId = userFromId;
            UserToId = userToId;
            Message = message;
        }
        public PrivateMessage() { }


    }
}