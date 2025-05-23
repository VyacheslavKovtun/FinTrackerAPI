using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinTrackerAPI.Services.Interfaces.Utils
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";


        public const string USER_AUDIENCE = "AuthSystemUser";

        private const string USER_KEY = "mysupersecret_system_secretkey!213";


        public const string SYSTEM_JWT_ACCESS_AUDIENCE = "AccessSystemUserJWT";

        private const string SYSTEM_JWT_ACESS_KEY = "2d00ab04-0e59-4a78-a642-3a0bff06b5a8";


        public const int LIFETIME = 43200; // 12 hours

        public static SymmetricSecurityKey GetSystemUserSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(USER_KEY));
        }

        public static SymmetricSecurityKey GetSystemJWTAccessSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SYSTEM_JWT_ACESS_KEY));
        }
    }
}
