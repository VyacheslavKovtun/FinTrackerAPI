using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinTrackerAPI.Services.Interfaces.Utils
{
    public class AuthOptions
    {
        public const string ISSUER = "FinTrackerAuthServer";


        public const string USER_AUDIENCE = "FinAuthSystemUser";

        private const string USER_KEY = "mysuperfinsecret_system_secretkey!213";


        public const string SYSTEM_JWT_ACCESS_AUDIENCE = "AccessSystemUserJWT";

        private const string SYSTEM_JWT_ACESS_KEY = "3b23be05-8t72-2a63-t152-4b5der81e4w1";


        public const int LIFETIME = 43200; // 12 hours

        public static SymmetricSecurityKey GetUserSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(USER_KEY));
        }

        public static SymmetricSecurityKey GetSystemJWTAccessSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SYSTEM_JWT_ACESS_KEY));
        }
    }
}
