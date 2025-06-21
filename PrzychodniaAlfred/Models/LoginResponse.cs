namespace PrzychodniaAlfred.Models
{
    public class LoginResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public User user { get; set; }
    }
}
