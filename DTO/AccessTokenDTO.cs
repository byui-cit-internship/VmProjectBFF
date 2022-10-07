namespace vmProjectBFF.DTO
{
    public class AccessTokenDTO
    {
        public string AccessTokenValue { get; set; }

        public AccessTokenDTO(string accessTokenValue)
        {
            AccessTokenValue = accessTokenValue;
        }
    }
}