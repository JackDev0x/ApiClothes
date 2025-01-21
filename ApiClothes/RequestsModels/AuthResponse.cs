namespace ApiClothes.RequestsModels
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int ExpiresIn { get; set; }
    }
}