namespace ApiClothes
{
    public class AuthenticationSettings
    {
        public string JwtKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}