namespace MvcMessageLogger.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string CoffeeOfChoice { get; set; } = string.Empty;
        public List<Message> Messages { get; } = new List<Message>();

        // This constructor is for ASP.NET Core's model binder
        public User() { }
        public User(string name, string username, string password, string coffeeOfChoice)
        {
            Name = name;
            Username = username;
            Password = password;
            CoffeeOfChoice = coffeeOfChoice;
        }
    }
}
