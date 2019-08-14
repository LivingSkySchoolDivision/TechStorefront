using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.Lib
{
    public static class PasswordSanitizer
    {
        private const char passwordReplaceCharacter = '*';

        public static string MaskPassword(this string givenString)
        {
            if (string.IsNullOrEmpty(givenString)) { return string.Empty; }

            StringBuilder returnMe = new StringBuilder();
            int charsToShow = 0;

            if (givenString.Length > 6)
            {
                charsToShow = 2;
            }
                        
            for(int x = 0; x < givenString.Length; x++)
            {
                if (x < charsToShow)
                {
                    returnMe.Append(givenString[x]);
                } else
                {
                    returnMe.Append(passwordReplaceCharacter);
                }
            }

            return returnMe.ToString();
        }
    }
}
