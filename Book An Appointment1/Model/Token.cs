namespace Book_An_Appointment1.Model
{
    public class Token
    {
    }
    public class TokenClient
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}
