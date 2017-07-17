/* Class that deals with saving and loading settings.
 * Copyright - Elliot Dawber 2017. MIT License. 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace KeyboardToControllerMapper
{
    /// <summary>
    /// Class that deals with saving and loading settings.
    /// </summary>
    class SettingsManager
    {
        /// <summary>
        /// Holds the key mappings after a read of the settings file.
        /// </summary>
        List<KeyMapping> keyMappings;

        /// <summary>
        /// Manages saving/loading key mappings.
        /// </summary>
        public SettingsManager()
        {
            keyMappings = new List<KeyMapping>();
        }


        /// <summary>
        /// Writes the settings given by *input* to the settings file.
        /// </summary>
        /// <param name="input">A List of key mappings to save.</param>
        /// <returns>Whether we successfully saved the mappings.</returns>
        public bool writeSettings(List<KeyMapping> input)
        {
            string output = "";
            foreach(KeyMapping map in input)
            {
                string nonLocale = Converter.LocaleToString(map.Key);
                output += "Map: " + nonLocale + ", " + map.Button + "\n";
            }

            try
            {
                File.WriteAllText(@"settings.txt", output);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }



        /// <summary>
        /// Reads the settings from the settings file. 
        /// Use getKeyMappings() to obtain the results.
        /// </summary>
        /// <returns>Whether we successfully read the settings file.</returns>
        public bool readSettings()
        {
            string[] input;
            try
            {
                input = File.ReadAllLines(@"settings.txt");
            }
            catch (Exception)
            {
                return false;
            }

            keyMappings.Clear();

            foreach (string line in input)
            {
                // Try to split the line into its "KeyboardKey" and "ControllerButton" parts.
                string alteredLine = line.Replace(" ", string.Empty);
                if (alteredLine.StartsWith("Map:") && alteredLine.Contains(","))
                {
                    alteredLine = alteredLine.Substring(4);
                    string[] parts = alteredLine.Split(',');
                    if (parts.Length != 2)
                    {
                        continue;
                    }
                    // Check whether both parts are a valid key and a valid controller button.
                    try
                    {
                        Keys parsedKey = (Keys)Enum.Parse(typeof(Keys), parts[0]);
                        ControllerButtons parsedButton = (ControllerButtons)Enum.Parse(typeof(ControllerButtons), parts[1]);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    // Successfully found a valid mapping.
                    string localeString = Converter.StringToLocale(parts[0]);
                    keyMappings.Add(new KeyMapping() { Key = localeString, Button = parts[1] });
                }
            }
            return true;
        }


        
        /// <summary>
        /// Returns the key mappings read from the settings file. Should only be used when readSettings has returned true.
        /// </summary>
        /// <returns>A List of the key mappings read from the settings file.</returns>
        public List<KeyMapping> getKeyMappings()
        {
            return keyMappings;  
        }
    }
}
