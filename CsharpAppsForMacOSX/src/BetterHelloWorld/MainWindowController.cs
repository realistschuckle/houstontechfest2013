/******************************************************************************
The MIT License (MIT)

Copyright (c) 2013 Curtis Schlak

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace BetterHelloWorld
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{
		/// <summary>
		/// Subscribe to the <see cref="PushMeButton"/>'s
		/// <see cref="MonoMac.AppKit.NSButton.Activated"/> event
		/// and set the value of the <see cref="MessageLabel"/>'s
		/// <see cref="MonoMac.AppKit.NSTextField.StringValue"/>.
		/// </summary>
		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			PushMeButton.Activated += (object sender, EventArgs ea) => {
				MessageLabel.StringValue = "Don't sweat it!";
			};
		}

		/// <summary>
		/// Handle the clear text action by setting the value of the
		/// <see cref="MessageLabel"/>'s
		/// <see cref="MonoMac.AppKit.NSTextField.StringValue"/>.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		partial void clearText (MonoMac.Foundation.NSObject sender)
		{
			MessageLabel.StringValue = "";
		}

		#region Constructors

		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{
		}
		#endregion

		//strongly typed window accessor
		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}
	}
}

