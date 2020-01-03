// Copyright (c) 2017 Leacme (http://leac.me). View LICENSE.md for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Leacme.Lib.VeryMeta;

namespace Leacme.App.VeryMeta {

	public class AppUI {

		private StackPanel rootPan = (StackPanel)Application.Current.MainWindow.Content;
		private Library lib = new Library();

		public AppUI() {

			var oF = App.HorizontalFieldWithButton;
			oF.label.Text = "Open media file:";
			oF.holder.HorizontalAlignment = HorizontalAlignment.Center;
			oF.field.IsReadOnly = true;
			oF.field.Width = 600;
			oF.button.Content = "Open...";

			var bl1 = App.TextBlock;
			bl1.Text = "Media file metadata:";
			bl1.Margin = new Thickness(85, App.Margin.Top, App.Margin.Right, App.Margin.Bottom);

			var dgm = App.DataGrid;
			dgm.Height = 400;
			dgm.AutoGeneratingColumn += (z, zz) => {
				zz.Column.Header = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", Regex.Matches(
						(string)zz.Column.Header, @"(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+|[0-9\.*]+|[a-z]+)").OfType<Match>().Select(
							zzz => zzz.Value).ToArray()));
			};

			dgm.Items = new List<(string directoryName, string name, string description)>().Select(z => new { z.directoryName, z.name, z.description });
			oF.button.Click += async (z, zz) => {
				oF.field.Text = (await OpenFile()).FirstOrDefault();
				if (!string.IsNullOrWhiteSpace(oF.field.Text)) {
					var md = lib.GetFileMetadata(new Uri(oF.field.Text));
					dgm.Items = md.Select(zzz => new { zzz.directoryName, zzz.name, zzz.description });
				}
			};

			rootPan.Children.AddRange(new List<IControl> { oF.holder, new Control { Height = 10 }, bl1, dgm });
		}

		private async Task<IEnumerable<string>> OpenFile() {
			var dialog = new OpenFileDialog() {
				Title = "Open Media File...",
				InitialDirectory = Directory.GetCurrentDirectory(),
				AllowMultiple = false,
			};
			var res = await dialog.ShowAsync(Application.Current.MainWindow);
			if (res?.Any() == true) {
				Directory.SetCurrentDirectory(Path.GetDirectoryName(res.First()));
			}

			return (res?.Any() == true) ? res : Enumerable.Empty<string>();
		}
	}
}