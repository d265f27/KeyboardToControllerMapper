/* Basic conversions between strings, key enums, and integer representations of key scancodes.
 * Terminology: 
 *      Key - A System.Windows.Forms.Keys enum.
 *      Int - The integer value of the above enum.
 *      String - String representation of the above enum, e.g. "A" or "Oemtilde".
 *      Locale - A localised string representation of the enum.
 *               Format: "OfficialName / Character", Character is optional,
 *               e.g. "Oemtilde / `" (which is localised).
 * Copyright - Elliot Dawber 2017. MIT License.
 */

using System;
using System.Text;
using System.Windows.Forms;

// For Keyboard hooks.
using System.Runtime.InteropServices;

namespace KeyboardToControllerMapper
{
    class Converter
    {

        // Translates a virtual-key code to a unicode string.
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, StringBuilder pwszBuff, int cchBuff, uint wFlags);


        /// <summary>
        /// Converts a string of the enum value (e.g. "Oemtilde") to a Keys enum.
        /// </summary>
        /// <param name="input">String of the enum value (e.g. "Oemtilde").</param>
        /// <returns>A Keys enum.</returns>
        public static Keys StringToKeys(string input)
        {
            return (Keys)Enum.Parse(typeof(Keys), input);
        }


        /// <summary>
        /// Converts a string of the enum value (e.g. "Oemtilde") to a Locale string (e.g. "Oemtilde / `").
        /// </summary>
        /// <param name="input">String of the enum value (e.g. "Oemtilde").</param>
        /// <returns>A Locale string (e.g. "Oemtilde / `").</returns>
        public static string StringToLocale(string input)
        {
            return KeysToLocale(StringToKeys(input));
        }


        /// <summary>
        /// Converts a string of the enum value (e.g. "Oemtilde") to the int value of the Keys enum.
        /// </summary>
        /// <param name="input">String of the enum value (e.g. "Oemtilde").</param>
        /// <returns>The int value of the Keys enum.</returns>
        public static int StringToInt(string input)
        {
            return (int)Enum.Parse(typeof(Keys), input);
        }


        /// <summary>
        /// Converts a Keys enum to the int value of it.
        /// </summary>
        /// <param name="key">A Keys enum.</param>
        /// <returns>The int value of the Keys enum.</returns>
        public static int KeysToInt(Keys key)
        {
            return (int)key;
        }


        /// <summary>
        /// Converts a Keys enum to a Locale string (e.g. "Oemtilde / `").
        /// </summary>
        /// <param name="key">A Keys enum.</param>
        /// <returns>A Locale string (e.g. "Oemtilde / `").</returns>
        public static string KeysToLocale(Keys key)
        {
            byte[] byteArray = new byte[256];

            StringBuilder output = new StringBuilder(16);
            int result;
            unsafe
            {
                result = ToUnicode((uint)key, (uint)0, byteArray, output, (int)16, (uint)0);
            }
            
            if (result > 0)
            {
                // We don't want a multi-line string.
                if (key == Keys.Return)
                {
                    return key.ToString();
                }
                return key.ToString() + " / " + output;
            }
            else
            {
                return key.ToString();
            }
        }

        
        /// <summary>
        /// Converts a Keys enum to a string of the enum value (e.g. "Oemtilde").
        /// </summary>
        /// <param name="key">A Keys enum.</param>
        /// <returns>String of the enum value (e.g. "Oemtilde").</returns>
        public static string KeysToString(Keys key)
        {
            return key.ToString();
        }


        /// <summary>
        /// Convert the int value of the Keys enum to a Keys enum.
        /// </summary>
        /// <param name="keyCode">The int value of the Keys enum.</param>
        /// <returns>A Keys enum.</returns>
        public static Keys IntToKeys(int keyCode)
        {
            return (Keys)keyCode;
        }

        
        /// <summary>
        /// Converts the int value of the Keys enum to a string of the enum value (e.g. "Oemtilde").
        /// </summary>
        /// <param name="keyCode">The int value of the Keys enum.</param>
        /// <returns>String of the enum value (e.g. "Oemtilde").</returns>
        public static string IntToString(int keyCode)
        {
            return ((Keys)keyCode).ToString();
        }


        /// <summary>
        /// Converts the int value of the Keys enum to a Locale string (e.g. "Oemtilde / `").
        /// </summary>
        /// <param name="keyCode">The int value of the Keys enum.</param>
        /// <returns>A Locale string (e.g. "Oemtilde / `").</returns>
        public static string IntToLocale(int keyCode)
        {
            return KeysToLocale((Keys)keyCode);
        }


        /// <summary>
        /// Converts a Locale string (e.g. "Oemtilde / `") to a string of the enum value (e.g. "Oemtilde").
        /// </summary>
        /// <param name="locale">A Locale string (e.g. "Oemtilde / `").</param>
        /// <returns>String of the enum value (e.g. "Oemtilde").</returns>
        public static string LocaleToString(string locale)
        {
            int spaceLocation = locale.IndexOf(' ');
            if (spaceLocation == -1)
            {
                return locale;
            }
            else
            {
                return locale.Substring(0, spaceLocation);
            }
        }


        /// <summary>
        /// Converts a Locale string (e.g. "Oemtilde / `") to a Keys enum.
        /// </summary>
        /// <param name="locale">A Locale string (e.g. "Oemtilde / `").</param>
        /// <returns>A Keys enum.</returns>
        public static Keys LocaleToKeys(string locale)
        {
            return StringToKeys(LocaleToString(locale));
        }


        /// <summary>
        /// Converts a Locale string (e.g. "Oemtilde / `") to the int value of the Keys enum.
        /// </summary>
        /// <param name="locale">A Locale string (e.g. "Oemtilde / `").</param>
        /// <returns>The int value of the Keys enum.</returns>
        public static int LocaleToInt(string locale)
        {
            return (int)LocaleToKeys(locale);
        }

    }
}
