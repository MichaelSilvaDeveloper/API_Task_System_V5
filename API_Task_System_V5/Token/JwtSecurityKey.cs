using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API_Task_System_V5.Token
{
    public class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}