/*
****************************************************************************
*  Copyright (c) 2023,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

dd/mm/2023	1.0.0.1		XXX, Skyline	Initial version
****************************************************************************
*/

using System.Collections.Concurrent;
using Skyline.DataMiner.Utils.InteractiveAutomationScript;

namespace DOJO_1
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using Skyline.DataMiner.Automation;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		private InteractiveController _controller;

		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			// engine.ShowUI();

			try
			{
				_controller = new InteractiveController(engine);
				var dialog = new MyDialog(_controller);

				_controller.Run(dialog);
			}
			catch (ScriptAbortException)
			{
				throw;
			}
			catch (Exception e)
			{
				engine.ExitFail("Something went wrong: " + e);
			}
		}
	}

	internal class MyDialog : Dialog
	{
		private InteractiveController _controller;

		public MyDialog(InteractiveController controller) : base(controller.Engine)
		{
			_controller = controller;
			Title = "Add Fields";
			myButton = new Button("+");
			AddWidget(myButton, 0, 0);

			myButton.Pressed += Button_Pressed;
		}

		public Button myButton { get; set; }

		private object myLock = new object();
		private readonly List<TextBox> myTextBoxes = new List<TextBox>();

		public void Button_Pressed(object sender, EventArgs e)
		{
			lock (myLock)
			{
				var tb = new TextBox($"Field {myTextBoxes.Count + 1}");
				AddWidget(tb, myTextBoxes.Count, 1);
				myTextBoxes.Add(tb);
			}
		}
	}
}