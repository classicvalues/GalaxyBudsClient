﻿using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using GalaxyBudsClient.Bluetooth;
using GalaxyBudsClient.Interface.Elements;
using GalaxyBudsClient.Interface.Items;
using GalaxyBudsClient.Model.Constants;
using GalaxyBudsClient.Platform;
using GalaxyBudsClient.Utils.DynamicLocalization;
using Serilog;

namespace GalaxyBudsClient.Interface.Pages
{
 	public class ConnectionLostPage : AbstractPage
	{
		public override Pages PageType => Pages.NoConnection;

		private readonly TextBlock _additionalInfo;
		private readonly LoadingSpinner _loadingSpinner;
		private readonly IconListItem _retry;

		private string ErrorDescription
		{
			set => _additionalInfo.Text = (value == string.Empty) ?
				Loc.Resolve("connlost_noinfo") : value;
			get => _additionalInfo.Text;
		}
		
		public ConnectionLostPage()
		{   
			AvaloniaXamlLoader.Load(this);

			_loadingSpinner = this.FindControl<LoadingSpinner>("LoadingSpinner");
			_additionalInfo = this.FindControl<TextBlock>("AdditionalInfo");
			_retry = this.FindControl<IconListItem>("Retry");
			
			ErrorDescription = string.Empty;

			BluetoothImpl.Instance.Disconnected += (sender, s) =>
			{
				Dispatcher.UIThread.Post(() =>
				{
					ErrorDescription = s;
                    ResetRetryButton();
				}, DispatcherPriority.Render);
			};
			BluetoothImpl.Instance.BluetoothError += (sender, s) =>
			{
				Dispatcher.UIThread.Post(() =>
				{
					ResetRetryButton();
					
					if (s.ErrorCode == BluetoothException.ErrorCodes.MessageNotSent)
					{
						/* Hide "message couldn't be sent" because it'll shadow the actual error in most situations */
						return;
					}
					
					ErrorDescription = s.Message;
				}, DispatcherPriority.Render);
			};
			BluetoothImpl.Instance.Connected += (sender, args) =>
			{
				Dispatcher.UIThread.Post(ResetRetryButton, DispatcherPriority.Render);
			};
		}
		

		public void ResetRetryButton()
		{
			_loadingSpinner.IsVisible = false;
			_retry.IsEnabled = true;
			_retry.Text = Loc.Resolve("connlost_connect");
		}

		public override void OnPageHidden()
		{
			ResetRetryButton();
		}

		private void Retry_OnPointerPressed(object? sender, PointerPressedEventArgs e)
		{
			if (_retry.IsEnabled)
			{
				_loadingSpinner.IsVisible = true;
				_retry.IsEnabled = false;
				_retry.Text = Loc.Resolve("connlost_connecting");

				if (!BluetoothImpl.Instance.RegisteredDeviceValid)
				{
					MainWindow.Instance.Pager.SwitchPage(Pages.Welcome);
					return;
				}

				BluetoothImpl.Instance.ConnectAsync()
					.ContinueWith((_)=>MainWindow.Instance.Pager.SwitchPage(Pages.Home));
				;
			}
		}
	}
}
