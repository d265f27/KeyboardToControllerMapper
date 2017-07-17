/* Main class for KeyboardToControllerMapper.
 * Provides GUI for selecting mappings, and responds to keyboard
 * presses and initiates controller presses.
 * Based on code SCPTester from Morgan Zolob (Mogzol) MIT License, see 
 * github.com/mogzol/ScpDriverInterface
 * Any modifications copyright Elliot Dawber 2017, MIT License.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ScpDriverInterface;
using System.Runtime.InteropServices;


namespace KeyboardToControllerMapper
{
    public partial class KeyboardToControllerMapper : Form
    {
        private ScpBus _scpBus;
        private X360Controller _controller = new X360Controller();
        private InterceptKeys _interceptKeys;
        private SettingsManager _settingsManager;

        private byte[] _outputReport = new byte[8];

        /// <summary>
        /// Holds the recent status updates.
        /// </summary>
        private string[] statuses = new string[10];

        /// <summary>
        /// Holds the current mappings between keys and controller buttons for display.
        /// </summary>
        BindingList<KeyMapping> MappingList = new BindingList<KeyMapping>();


        // For keyboard hooks.
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        /// <summary>
        /// Holds the Locale strings for the key selection list box.
        /// </summary>
        private KeyListBoxItem[] keyListBoxItems;

        /// <summary>
        /// An array of every possible Keys key, somewhat sorted with oddities at the end.
        /// </summary>
        private Keys[] keysOriginal = {
                Keys.A, Keys.Add, Keys.Apps, Keys.Attn, Keys.B, Keys.Back, Keys.BrowserBack, Keys.BrowserFavorites,
                Keys.BrowserForward, Keys.BrowserHome, Keys.BrowserRefresh, Keys.BrowserSearch, Keys.BrowserStop, Keys.C,
                Keys.Cancel, Keys.Capital, Keys.CapsLock, Keys.Clear, Keys.ControlKey, Keys.Crsel, Keys.D, Keys.D0, Keys.D1,
                Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.Decimal, Keys.Delete,
                Keys.Divide, Keys.Down, Keys.E, Keys.End, Keys.Enter, Keys.EraseEof, Keys.Escape, Keys.Execute, Keys.Exsel,
                Keys.F, Keys.F1, Keys.F10, Keys.F11, Keys.F12, Keys.F13, Keys.F14, Keys.F15, Keys.F16, Keys.F17, Keys.F18,
                Keys.F19, Keys.F2, Keys.F20, Keys.F21, Keys.F22, Keys.F23, Keys.F24, Keys.F3, Keys.F4, Keys.F5, Keys.F6,
                Keys.F7, Keys.F8, Keys.F9, Keys.FinalMode, Keys.G, Keys.H, Keys.HanguelMode, Keys.HanjaMode, Keys.Help,
                Keys.Home, Keys.I, Keys.IMEAccept, Keys.IMEAceept, Keys.IMEConvert, Keys.IMEModeChange, Keys.IMENonconvert,
                Keys.Insert, Keys.J, Keys.JunjaMode, Keys.K, Keys.KanaMode, Keys.KanjiMode, Keys.L, Keys.LaunchApplication1,
                Keys.LaunchApplication2, Keys.LaunchMail, Keys.LButton, Keys.LControlKey, Keys.Left, Keys.LineFeed,
                Keys.LMenu, Keys.LShiftKey, Keys.LWin, Keys.M, Keys.MButton, Keys.MediaNextTrack, Keys.MediaPlayPause,
                Keys.MediaPreviousTrack, Keys.MediaStop, Keys.Menu, Keys.Multiply, Keys.N, Keys.Next, Keys.NoName,
                Keys.None, Keys.NumLock, Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5,
                Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.O, Keys.Oem1, Keys.Oem102, Keys.Oem2,
                Keys.Oem3, Keys.Oem4, Keys.Oem5, Keys.Oem6, Keys.Oem7, Keys.Oem8, Keys.OemBackslash, Keys.OemClear,
                Keys.OemCloseBrackets, Keys.Oemcomma, Keys.OemMinus, Keys.OemOpenBrackets, Keys.OemPeriod, Keys.OemPipe,
                Keys.Oemplus, Keys.OemQuestion, Keys.OemQuotes, Keys.OemSemicolon, Keys.Oemtilde, Keys.P, Keys.Pa1,
                Keys.Packet, Keys.PageDown, Keys.PageUp, Keys.Pause, Keys.Play, Keys.Print, Keys.PrintScreen, Keys.Prior,
                Keys.ProcessKey, Keys.Q, Keys.R, Keys.RButton, Keys.RControlKey, Keys.Return, Keys.Right, Keys.RMenu,
                Keys.RShiftKey, Keys.RWin, Keys.S, Keys.Scroll, Keys.Select, Keys.SelectMedia, Keys.Separator,
                Keys.ShiftKey, Keys.Sleep, Keys.Snapshot, Keys.Space, Keys.Subtract, Keys.T, Keys.Tab, Keys.U, Keys.Up,
                Keys.V, Keys.VolumeDown, Keys.VolumeMute, Keys.VolumeUp, Keys.W, Keys.X, Keys.XButton1, Keys.XButton2,
                Keys.Y, Keys.Z, Keys.Zoom
            };

        /// <summary>
        /// Contains the mappings for normal keys to controller buttons, used by keyboard callback.
        /// </summary>
        private ControllerButtons[] mapping = new ControllerButtons[256];

        // Sticks are axes, for we can't hold left and right, so we need to track if they're 
        // being held so we know what the other extreme will do. So releasing right will make it left
        // if left is held and not middle.
        private bool leftStickLeftHeld = false;
        private bool leftStickRightHeld = false;
        private bool leftStickUpHeld = false;
        private bool leftStickDownHeld = false;

        private bool rightStickLeftHeld = false;
        private bool rightStickRightHeld = false;
        private bool rightStickUpHeld = false;
        private bool rightStickDownHeld = false;

        /// <summary>
        /// Whether we are searching for a key press for the "Find By Key Press" button.
        /// </summary>
        private bool findingKey = false;

        private void clearMappings()
        {
            for (int i = 0; i < mapping.Length; i++)
            {
                mapping[i] = ControllerButtons.ButtonNone;
            }
        }

        private void updateMapping(Keys key, ControllerButtons button)
        {
            int keyCode = (int)key;
            if (keyCode < 256)
            {
                mapping[keyCode] = button;
            }
        }

        public void keyboardCallback(IntPtr wParam, IntPtr lParam)
        {
            int keyCode = Marshal.ReadInt32(lParam);

            if (keyCode > 0)
            {
                // Easy Key Finding code.
                if (findingKey)
                {
                    easyFindKeyCallback(keyCode);
                }

                // Determining which button it is mapped to, if any.
                ControllerButtons mappedButton = ControllerButtons.ButtonNone;

                if (keyCode < 256)
                {
                    mappedButton = mapping[keyCode];
                }

                // Determine if it is pressed or released.
                bool pressed = false;
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    pressed = true;
                }

                // Press the button it is mapped to.
                switch (mappedButton)
                {
                    case ControllerButtons.ButtonNone:
                        break;
                    case ControllerButtons.ButtonA:
                        handleButtonA(pressed);
                        break;
                    case ControllerButtons.ButtonB:
                        handleButtonB(pressed);
                        break;
                    case ControllerButtons.ButtonX:
                        handleButtonX(pressed);
                        break;
                    case ControllerButtons.ButtonY:
                        handleButtonY(pressed);
                        break;
                    case ControllerButtons.DpadLeft:
                        handleDpadLeft(pressed);
                        break;
                    case ControllerButtons.DpadRight:
                        handleDpadRight(pressed);
                        break;
                    case ControllerButtons.DpadUp:
                        handleDpadUp(pressed);
                        break;
                    case ControllerButtons.DpadDown:
                        handleDpadDown(pressed);
                        break;
                    case ControllerButtons.BumperLeft:
                        handleBumperLeft(pressed);
                        break;
                    case ControllerButtons.BumperRight:
                        handleBumperRight(pressed);
                        break;
                    case ControllerButtons.ButtonXbox:
                        handleButtonXbox(pressed);
                        break;
                    case ControllerButtons.ButtonStart:
                        handleButtonStart(pressed);
                        break;
                    case ControllerButtons.ButtonBack:
                        handleButtonBack(pressed);
                        break;
                    case ControllerButtons.LeftStickClick:
                        handleLeftStickClick(pressed);
                        break;
                    case ControllerButtons.LeftStickLeft:
                        handleLeftStickLeft(pressed);
                        break;
                    case ControllerButtons.LeftStickRight:
                        handleLeftStickRight(pressed);
                        break;
                    case ControllerButtons.LeftStickUp:
                        handleLeftStickUp(pressed);
                        break;
                    case ControllerButtons.LeftStickDown:
                        handleLeftStickDown(pressed);
                        break;
                    case ControllerButtons.RightStickClick:
                        handleRightStickClick(pressed);
                        break;
                    case ControllerButtons.RightStickLeft:
                        handleRightStickLeft(pressed);
                        break;
                    case ControllerButtons.RightStickRight:
                        handleRightStickRight(pressed);
                        break;
                    case ControllerButtons.RightStickUp:
                        handleRightStickUp(pressed);
                        break;
                    case ControllerButtons.RightStickDown:
                        handleRightStickDown(pressed);
                        break;
                    case ControllerButtons.TriggerLeft:
                        handleTriggerLeft(pressed);
                        break;
                    case ControllerButtons.TriggerRight:
                        handleTriggerRight(pressed);
                        break;
                    default:
                        break;
                }
            }
        }

        public KeyboardToControllerMapper()
        {
            InitializeComponent();

            initialiseStatuses();
            addStatus("Initialising.");

            // Initialise internal mappings.
            clearMappings();
            
            // Initialise our keys to the proper localisation.
            keyListBoxItems = new KeyListBoxItem[keysOriginal.Length];
            for (int i = 0; i < keysOriginal.Length; i++)
            {
                keyListBoxItems[i] = new KeyListBoxItem(keysOriginal[i], Converter.KeysToLocale(keysOriginal[i]));
            }
            newKeyListBox.DataSource = keyListBoxItems;
            newKeyListBox.DisplayMember = "localeString";

            _settingsManager = new SettingsManager();
            loadSettings();

            // Mapping list initialisation.
            MappingDataGridView.DataSource = MappingList;
            DataGridViewColumn column1 = MappingDataGridView.Columns[0];
            DataGridViewColumn column2 = MappingDataGridView.Columns[1];
            column1.Width = 70;
            column2.Width = 70;

            _interceptKeys = new InterceptKeys(new InterceptKeys.KeyboardCallback(keyboardCallback));

            controllerNum.Tag = controllerNum.Value;

            try
            {
                _scpBus = new ScpBus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to connect to driver, please check that it is installed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            pluginController(1);


        }













        




        //// UI Code
        
        // Deals with the button press of the map key button.
        // <summary> Call back for the button press of the map key button.</summary>
        private void mapKeyButtonClick(object sender, EventArgs e)
        {
            // Key is keyboard.
            KeyListBoxItem selectedKeyItem = (KeyListBoxItem)newKeyListBox.SelectedItem;
            if (selectedKeyItem == null)
            {
                addStatus("Please select a key.");
                return;
            }
            Keys selectedKey = selectedKeyItem.Key;
            int selectedKeyCode = Converter.KeysToInt(selectedKey);
            string selectedKeyString = Converter.LocaleToString(selectedKeyItem.LocaleString);
            string selectedKeyLocale = selectedKeyItem.LocaleString;

            // Button is controller.
            string selectedButtonString = (string)buttonListBox.SelectedItem;
            if (selectedButtonString == null)
            {
                addStatus("Please select a controller button.");
                return;
            }
            ControllerButtons selectedButton = (ControllerButtons)Enum.Parse(typeof(ControllerButtons), selectedButtonString);
            int selectedButtonCode = (int)selectedButton;

            // Check if the key is already mapped to something.
            for (int i = 0; i < MappingList.Count; i++)
            {
                if (MappingList[i].Key == selectedKeyLocale)
                {
                    MappingList[i].Button = selectedButtonString;
                    MappingList[i] = new KeyMapping() { Key = selectedKeyLocale, Button = selectedButtonString };
                    updateMapping(selectedKey, selectedButton);
                    addStatus("Changed mapping of " + selectedKeyString + " to " + selectedButtonString);
                    return;
                }
            }

            // We have not found a previous mapping then.
            addMappingToUI(selectedKeyLocale, selectedButtonString);
            updateMapping(selectedKey, selectedButton);
            addStatus("Mapped " + selectedKeyString + " to " + selectedButtonString);
        }

        

        /// <summary>
        /// Deals with the button press of the easy find key button.
        /// Other functions do work for this whole ability also.
        /// </summary>
        private void easyFindKeyButtonClick(object sender, EventArgs e)
        {
            if (findingKey) {
                // Already finding the key, stop.
                findingKey = false;
                easyFindKeyButton.BackColor = Color.Transparent;
                easyFindKeyButton.Text = "Find By Key Press";
            } else {
                // Not finding the key, start.
                findingKey = true;
                easyFindKeyButton.BackColor = Color.Salmon;
                easyFindKeyButton.Text = "Press Key";
            }
        }



        /// <summary> 
        /// The callback for when a key has been pressed after the easyFindKeyButton has been pressed.
        /// </summary>
        private void easyFindKeyCallback(int keyCode)
        {
            Keys key = Converter.IntToKeys(keyCode);
            // Find the index of our key, and select that key in our list.
            int result = -1;
            for (int i = 0; i < keysOriginal.Length; i++)
            {
                if (keysOriginal[i] == key)
                {
                    result = i;
                }
            }
            if (result == -1)
            {
                addStatus("Could not detect key properly.");
            } else
            {
                newKeyListBox.SetSelected(result, true);
            }
            findingKey = false;
            easyFindKeyButton.BackColor = Color.Transparent;
            easyFindKeyButton.Text = "Find By Key Press";
        }


        /// <summary>
        /// Deals with the button click from the reset mappings button.
        /// </summary>
        private void resetMappingsButtonClick(object sender, EventArgs e)
        {
            MappingList.Clear();
            clearMappings();
            addStatus("Reset mappings.");
        }



        /// <summary>
        /// Deals with the button click from the remove mapping button.
        /// </summary>
        private void removeMappingButtonClick(object sender, EventArgs e)
        {
            if (MappingDataGridView.CurrentCell == null)
            {
                addStatus("Please select an item.");
                return;
            }
            int index = MappingDataGridView.CurrentCell.RowIndex;
            Keys selectedKey = Converter.LocaleToKeys(MappingList[index].Key);
            
            updateMapping(selectedKey, ControllerButtons.ButtonNone);
            MappingList.RemoveAt(MappingDataGridView.CurrentCell.RowIndex);
            addStatus("Removed mapping.");
        }



        /// <summary>
        /// Adds a mapping to the UI
        /// </summary>
        private void addMappingToUI(string locale, string button)
        {
            MappingList.Add(new KeyMapping() { Key = locale, Button = button });
        }



        /// <summary>
        /// Initialises the statuses.
        /// </summary>
        private void initialiseStatuses()
        {
            for (int i = 0; i < statuses.Length; i++)
            {
                statuses[i] = "";
            }
            updateStatusLabel();
        }



        /// <summary>
        /// Add a new status to the bottom.
        /// </summary>
        /// <param name="status">The status to add.</param>
        private void addStatus(string status)
        {
            for (int i = 0; i < statuses.Length - 1; i++)
            {
                statuses[i] = statuses[i + 1];
            }
            statuses[statuses.Length - 1] = status;
            updateStatusLabel();
        }



        /// <summary>
        /// Reshow the status label with the current statuses saved.
        /// </summary>
        private void updateStatusLabel()
        {
            string combinedStatus = "Status:";
            for (int i = 0; i < statuses.Length; i++)
            {
                if (statuses[i].Length > 0)
                {
                    combinedStatus += "\n" + statuses[i];
                }
            }
            statusLabel.Text = combinedStatus;
        }



        /// <summary>
        /// Runs on program close.
        /// </summary>
        private void onClose(object sender, FormClosingEventArgs e)
        {
            unplugAllControllers();
        }



        /// <summary>
        /// Loads the settings from the settings file.
        /// </summary>
        private void loadSettings()
        {
            // So we call readSettings.
            bool successful = _settingsManager.readSettings();
            if (!successful)
            {
                addStatus("Could not load settings.");
                return;
            }
            // Then if that succeeded then we get the resulting list.
            List<KeyMapping> tempList = _settingsManager.getKeyMappings();

            // Then we remove all current mappings.
            MappingList.Clear();
            clearMappings();

            // Then we add all new mappings.
            foreach (KeyMapping keymapping in tempList)
            {
                addMappingToUI(keymapping.Key, keymapping.Button);
                ControllerButtons button = (ControllerButtons)Enum.Parse(typeof(ControllerButtons), keymapping.Button);
                Keys key = Converter.LocaleToKeys(keymapping.Key);
                updateMapping(key, button);
            }

            addStatus("Loaded settings.");
        }



        /// <summary>
        /// Callback for when the load button is clicked.
        /// </summary>
        private void loadButtonClick(object sender, EventArgs e)
        {
            loadSettings();
        }



        /// <summary>
        /// Callback for when the save button is clicked.
        /// </summary>
        private void saveButtonClick(object sender, EventArgs e)
        {
            bool result = _settingsManager.writeSettings(MappingList.ToList());
            if (result)
            {
                addStatus("Saved settings.");
            }
            else
            {
                addStatus("Could not save settings.");
            }
        }


        /// <summary>
        /// Plugs in a controller.
        /// </summary>
        /// <param name="controller">Controller number to plug in.</param>
        private void pluginController(int controller)
        {
            bool result = _scpBus.PlugIn(controller);
            if (result)
            {
                addStatus("Successfully plugged in controller.");
            }
            else
            {
                addStatus("Could not plug in controller.");
            }
        }


        /// <summary>
        /// Callback for when the plugin button is clicked.
        /// </summary>
        private void plugin_Click(object sender, EventArgs e)
        {
            pluginController((int)controllerNum.Value);
        }


        /// <summary>
        /// Unplugs a controller.
        /// </summary>
        /// <param name="controller">Controller number to unplug.</param>
        private void unplugController(int controller)
        {
            bool result = _scpBus.Unplug(controller);
            if (result)
            {
                _controller = new X360Controller();
                addStatus("Successfully unplugged controller.");
            }
            else
            {
                addStatus("Could not unplug controller.\n - Maybe it wasn't plugged in?");
            }
        }



        /// <summary>
        /// Callback for when the unplug button is clicked.
        /// </summary>
        private void unplug_Click(object sender, EventArgs e)
        {
            unplugController((int)controllerNum.Value);
        }



        /// <summary>
        /// Unplugs all controllers.
        /// </summary>
        private void unplugAllControllers()
        {
            bool result = _scpBus.UnplugAll();
            if (result)
            {
                _controller = new X360Controller();
                addStatus("Successfully unplugged all controllers.");
            }
            else
            {
                addStatus("Could not unplug all controllers.\n - Were any plugged in?");
            }
        }



        /// <summary>
        /// Callback for when the unplug all button is clicked.
        /// </summary>
        private void unplugAll_Click(object sender, EventArgs e)
        {
            unplugAllControllers();
        }

























        private void handleButtonA(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.A;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.A);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleButtonB(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.B;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.B);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleButtonX(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.X;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.X);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleButtonY(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.Y;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.Y);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleDpadUp(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.Up;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.Up);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleDpadLeft(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.Left;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.Left);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleDpadRight(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.Right;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.Right);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleDpadDown(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.Down;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.Down);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleBumperLeft(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.LeftBumper;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.LeftBumper);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleBumperRight(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.RightBumper;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.RightBumper);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleButtonXbox(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.Logo;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.Logo);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleButtonStart(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.Start;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.Start);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleButtonBack(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.Back;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.Back);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleLeftStickClick(bool pressed)
        {
            if (pressed)
            {
                _controller.Buttons |= X360Buttons.LeftStick;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.LeftStick);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleRightStickClick(bool pressed)
        {

            if (pressed)
            {
                _controller.Buttons |= X360Buttons.RightStick;
            }
            else
            {
                _controller.Buttons &= ~(X360Buttons.RightStick);
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleTriggerLeft(bool pressed)
        {
            if (pressed)
            {
                _controller.LeftTrigger = (byte) 255;
            }
            else
            {
                _controller.LeftTrigger = (byte) 0;
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);            
        }

        private void handleTriggerRight(bool pressed)
        {
            if (pressed)
            {
                _controller.RightTrigger = (byte)255;
            }
            else
            {
                _controller.RightTrigger = (byte)0;
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleLeftStickLeft(bool pressed)
        {
            if (pressed)
            {
                leftStickLeftHeld = true;
                // If we are holding the opposite direction as well we set direction to neutral.
                if (leftStickRightHeld)
                {
                    _controller.LeftStickX = (short)0;
                }
                else
                {
                    _controller.LeftStickX = short.MinValue;
                }
            }
            else
            {
                leftStickLeftHeld = false;
                // If we are holding the opposite direction as well we set direction to that.
                if (leftStickRightHeld)
                {
                    _controller.LeftStickX = short.MaxValue;
                }
                else
                {
                    _controller.LeftStickX = (short) 0;
                }
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleLeftStickRight(bool pressed)
        {
            if (pressed)
            {
                leftStickRightHeld = true;
                // If we are holding the opposite direction as well we set direction to neutral.
                if (leftStickLeftHeld)
                {
                    _controller.LeftStickX = (short)0;
                }
                else
                {
                    _controller.LeftStickX = short.MaxValue;
                }
            }
            else
            {
                leftStickRightHeld = false;
                // If we are holding the opposite direction as well we set direction to that.
                if (leftStickLeftHeld)
                {
                    _controller.LeftStickX = short.MinValue;
                }
                else
                {
                    _controller.LeftStickX = (short)0;
                }
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleLeftStickUp(bool pressed)
        {
            if (pressed)
            {
                leftStickUpHeld = true;
                // If we are holding the opposite direction as well we set direction to neutral.
                if (leftStickDownHeld)
                {
                    _controller.LeftStickY = (short)0;
                }
                else
                {
                    _controller.LeftStickY = short.MaxValue;
                }
            }
            else
            {
                leftStickUpHeld = false;
                // If we are holding the opposite direction as well we set direction to that.
                if (leftStickDownHeld)
                {
                    _controller.LeftStickY = short.MinValue;
                }
                else
                {
                    _controller.LeftStickY = (short)0;
                }
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleLeftStickDown(bool pressed)
        {
            if (pressed)
            {
                leftStickDownHeld = true;
                // If we are holding the opposite direction as well we set direction to neutral.
                if (leftStickUpHeld)
                {
                    _controller.LeftStickY = (short)0;
                }
                else
                {
                    _controller.LeftStickY = short.MinValue;
                }
            }
            else
            {
                leftStickDownHeld = false;
                // If we are holding the opposite direction as well we set direction to that.
                if (leftStickUpHeld)
                {
                    _controller.LeftStickY = short.MaxValue;
                }
                else
                {
                    _controller.LeftStickY = (short)0;
                }
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleRightStickLeft(bool pressed)
        {
            if (pressed)
            {
                rightStickLeftHeld = true;
                // If we are holding the opposite direction as well we set direction to neutral.
                if (rightStickRightHeld)
                {
                    _controller.RightStickX = (short)0;
                }
                else
                {
                    _controller.RightStickX = short.MinValue;
                }
            }
            else
            {
                rightStickLeftHeld = false;
                // If we are holding the opposite direction as well we set direction to that.
                if (rightStickRightHeld)
                {
                    _controller.RightStickX = short.MaxValue;
                }
                else
                {
                    _controller.RightStickX = (short)0;
                }
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleRightStickRight(bool pressed)
        {
            if (pressed)
            {
                rightStickRightHeld = true;
                // If we are holding the opposite direction as well we set direction to neutral.
                if (rightStickLeftHeld)
                {
                    _controller.RightStickX = (short)0;
                }
                else
                {
                    _controller.RightStickX = short.MaxValue;
                }
            }
            else
            {
                rightStickRightHeld = false;
                // If we are holding the opposite direction as well we set direction to that.
                if (rightStickLeftHeld)
                {
                    _controller.RightStickX = short.MinValue;
                }
                else
                {
                    _controller.RightStickX = (short)0;
                }
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleRightStickUp(bool pressed)
        {
            if (pressed)
            {
                rightStickUpHeld = true;
                // If we are holding the opposite direction as well we set direction to neutral.
                if (rightStickDownHeld)
                {
                    _controller.RightStickY = (short)0;
                }
                else
                {
                    _controller.RightStickY = short.MaxValue;
                }
            }
            else
            {
                rightStickUpHeld = false;
                // If we are holding the opposite direction as well we set direction to that.
                if (rightStickDownHeld)
                {
                    _controller.RightStickY = short.MinValue;
                }
                else
                {
                    _controller.RightStickY = (short)0;
                }
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void handleRightStickDown(bool pressed)
        {
            if (pressed)
            {
                rightStickDownHeld = true;
                // If we are holding the opposite direction as well we set direction to neutral.
                if (rightStickUpHeld)
                {
                    _controller.RightStickY = (short)0;
                }
                else
                {
                    _controller.RightStickY = short.MinValue;
                }
            }
            else
            {
                rightStickDownHeld = false;
                // If we are holding the opposite direction as well we set direction to that.
                if (rightStickUpHeld)
                {
                    _controller.RightStickY = short.MaxValue;
                }
                else
                {
                    _controller.RightStickY = (short)0;
                }
            }
            bool result = _scpBus.Report((int)controllerNum.Value, _controller.GetReport(), _outputReport);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }

    enum ControllerButtons
    {
        ButtonNone,
        ButtonA, ButtonB, ButtonX, ButtonY,
        DpadLeft, DpadRight, DpadUp, DpadDown,
        BumperLeft, BumperRight,
        ButtonXbox, ButtonStart, ButtonBack,
        LeftStickClick, LeftStickLeft, LeftStickRight, LeftStickUp, LeftStickDown,
        RightStickClick, RightStickLeft, RightStickRight, RightStickUp, RightStickDown,
        TriggerLeft, TriggerRight
    }
    
    
    
}

