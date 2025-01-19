using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Business.DataProtection
{
    public interface IDataProtection
    {
        string Protect(string text); //Returns a method to encrypted entered password.
        string UnProtect(string protectedText); //Returns a method to decrypt the password.

    }
}
