using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Formats.Asn1.AsnWriter;
using WinRT.Interop;
using Vanara.PInvoke;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace UnifiedCaptionHeight
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private double _dpi;
		private uint _dpiX;
		private double _magicRegistryNumber = 330;

		public MainPage()
		{
			this.InitializeComponent();
			GetDpi();
			_dpi = _dpiX / 96.00;
			displayDpi.Text = string.Format("{0}, or {1}%", _dpiX.ToString(), (_dpi * 100).ToString());
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			int captionHeight = -CalculateTheMagicNum();
			//HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics
			RegistryKey HKEY_CURRENT_USER = Registry.CurrentUser;
			RegistryKey ControlPanel = HKEY_CURRENT_USER.OpenSubKey("Control Panel", true);
			RegistryKey Desktop = ControlPanel.OpenSubKey("Desktop", true);
			RegistryKey WindowMetrics = Desktop.OpenSubKey("WindowMetrics", true);
			//Name:CaptionHeight DefaultValue:-330
			WindowMetrics.SetValue("CaptionHeight", captionHeight.ToString(), RegistryValueKind.String);
			//Tell users to sign out
			SignOutContentDialog();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			RegistryKey HKEY_CURRENT_USER = Registry.CurrentUser;
			RegistryKey ControlPanel = HKEY_CURRENT_USER.OpenSubKey("Control Panel", true);
			RegistryKey Desktop = ControlPanel.OpenSubKey("Desktop", true);
			RegistryKey WindowMetrics = Desktop.OpenSubKey("WindowMetrics", true);
			WindowMetrics.SetValue("CaptionHeight", "-330", RegistryValueKind.String);
			//Tell users to sign out
			SignOutContentDialog();
		}

		private int CalculateTheMagicNum()
		{
			double win32ActualHeight = Math.Ceiling(26 * _dpi + 4);
			double uwpActualHeight = 32 * _dpi;
			int result = (int)(_magicRegistryNumber / win32ActualHeight * uwpActualHeight);
			return result;
		}

		private void GetDpi()
		{
			IntPtr hWnd = WindowNative.GetWindowHandle(this);
			WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
			DisplayArea displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
			IntPtr hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);
			SHCore.GetDpiForMonitor(hMonitor, SHCore.MONITOR_DPI_TYPE.MDT_DEFAULT, out _dpiX, out uint _);
		}

		private async void SignOutContentDialog()
		{
			ContentDialog dialog = new ContentDialog();

			// XamlRoot must be set in the case of a ContentDialog running in a Desktop app
			dialog.XamlRoot = this.XamlRoot;
			dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
			dialog.Title = "Sign out now?";
			dialog.PrimaryButtonText = "Sign out now";
			dialog.SecondaryButtonText = "Sign out later";
			dialog.DefaultButton = ContentDialogButton.Primary;
			dialog.Content = "You need to sign out to apply the change!";
			await dialog.ShowAsync();
		}
	}
}