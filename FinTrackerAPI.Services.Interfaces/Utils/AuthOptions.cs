using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinTrackerAPI.Services.Interfaces.Utils
{
    public class AuthOptions
    {
        public const string ISSUER = "FinTrackerAuthServer";


        public const string USER_AUDIENCE = "FinAuthSystemUser";

        private const string USER_KEY = "mysuperlongfinsecretkey_123456789!";


        public const int LIFETIME = 43200; // 12 hours

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(USER_KEY));
        }
    }
}
