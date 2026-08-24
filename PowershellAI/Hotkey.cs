using System.Diagnostics;
using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;

namespace PowershellAI
{
    internal class Hotkey
    {
        //CTRL + SHIFT + P
        private static readonly KeyGesture OpenGesture = new KeyGesture(Key.P, ModifierKeys.Control | ModifierKeys.Shift);
        public event EventHandler<string> HotkeyFired;
        public Hotkey()
        {
            HotkeyManager.HotkeyAlreadyRegistered += HotkeyManager_AlreadyRegistered;
            try
            {
                HotkeyManager.Current.AddOrReplace("OpenHotkey", OpenGesture, OnHotkeyFired);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error registering hotkey: {ex.Message}");
            }
        }

        private void HotkeyManager_AlreadyRegistered(object sender, HotkeyAlreadyRegisteredEventArgs e)
        {
            Debug.WriteLine("Hotkey is already registered.");
        }

        private void OnHotkeyFired(object sender, HotkeyEventArgs e)
        {
            HotkeyFired?.Invoke(this, "CTRL+SHIFT+P");
            e.Handled = true;
        }

    }
}
