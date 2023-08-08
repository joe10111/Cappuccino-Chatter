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
        public string MostUsedWord(User user)
        {
            List<string> words = new List<string>();

                foreach (var message in user.Messages)
                {   // split each message into a list of words
                    foreach (var word in message.Content.Split())
                    {   // add all the words into the empty list we made above
                        words.Add(word);
                    }
                }
            

            var MostCommonWords = words.GroupBy(x => x)
                            .Select(x => new { word = x.Key, wordCount = x.Count() })
                            .OrderByDescending(x => x.wordCount)
                            .Take(1);


            return MostCommonWords.First().word;
        }

        public string HourWithMostMessages(User user)
        {
            var mostActiveHour = user.Messages
        // - Use LINQ methods to extract the hour component from each CreatedAt value.
        .GroupBy(messages => messages.CreatedAt.Hour)
        // - Use select to make an anonymous type containing hour and count of messages
        .Select(messageHourGroup => new
        {    // make hour feild and set it equal to messageHourGroup.key
            hour = messageHourGroup.Key,
            // make messageCount feild and set it equal to messageHourGroup.Count()
            messageCount = messageHourGroup.Count()
        }).OrderByDescending(groupedData => groupedData.messageCount)
        .FirstOrDefault();

            TimeSpan hourAsTimeSpan = TimeSpan.FromHours(mostActiveHour.hour);

            string formattedStringHour = hourAsTimeSpan.ToString("hh':'mm");

            return formattedStringHour;
        }

        public string CountOfCoffeeInMessages(User user)
        {
            var coffeeCount = user.Messages.FindAll(m => m.Content.ToLower().Contains("coffee")).Count();
            
            return coffeeCount.ToString();
        }


        public bool LogInUser(string userName, string password)
        {
            if(this.Username == userName)
            {
                if(this.Password == password)
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
