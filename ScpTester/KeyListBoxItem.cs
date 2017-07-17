/* A class that holds a Key and a LocaleString so that a
 * listbox can show the LocaleString but return a Key.
 * Copyright: Elliot Dawber 2017. MIT License.
 */

using System.Windows.Forms;

namespace KeyboardToControllerMapper
{
    ///<summary>
    /// A class that holds a Key and a LocaleString so that a
    /// listbox can show the LocaleString but return a Key.
    ///</summary>
    class KeyListBoxItem
    {
        public Keys Key { get; set; }
        public string LocaleString { get; set; }

        public KeyListBoxItem(Keys key, string locale)
        {
            Key = key;
            LocaleString = locale;
        }
    }
}
