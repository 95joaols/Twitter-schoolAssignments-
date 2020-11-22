namespace TwitterCore
{
    public class User
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Biography { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool BINARYBITDEFAULTZERO { get; set; }
        public string PasswordSalt { get; set; }

        public User() { }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}