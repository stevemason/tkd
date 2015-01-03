//	"Taekwon-Do Theory Assistant" by Steven Mason

//	Copyright (C) 2005 Steven Mason

//	This program is free software; you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation; either version 2 of the License, or
//	(at your option) any later version.

//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.

//	You should have received a copy of the GNU General Public License
//	along with this program; if not, write to the Free Software
//	Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

//	The author may be contacted at:  steve@taekwondotheory.co.uk

// created on 20/06/2005 at 16:50using Gtk;
using Gtk;
using System;

/////////////////////////////////////////////////////////////////////
//Displays an "OK" dialog box with an 'info' stock icon             /
/////////////////////////////////////////////////////////////////////
//Constructor takes following parameters:                           /
//  string title = window title                                     /
//  string question = bold wording                                  /
//  string explanation = explanatory text below question            /
/////////////////////////////////////////////////////////////////////

public class EndOfTestDialog : Dialog
{
	//title = window title
	//question = bold wording
	//explanation = explanatory text below question
	
	private string base_directory = System.AppDomain.CurrentDomain.BaseDirectory;
	private string icon_path;
	
	public EndOfTestDialog (string title, string question, string explanation) : base ()
	{
		this.Title = title;
		this.HasSeparator = false;
		this.BorderWidth = 6;
		this.Resizable = false;
		
		icon_path = System.IO.Path.Combine(base_directory, "icon.png");
		
		this.SetIconFromFile(icon_path);
		
		HBox h = new HBox();
		h.BorderWidth = 6;
		h.Spacing = 12;
		
		Image i = new Image();
		//i.SetFromStock (Stock.DialogInfo, IconSize.Dialog);
		i.SetAlignment (0.5F, 0);
		h.PackStart (i, false, false, 0);
		
		VBox v = new VBox();
		Label l = new Label("<span weight=\"bold\" size=\"larger\">" + question + "</span>");
		l.LineWrap = true;
		l.UseMarkup = true;
		l.Xalign = 0;
		l.Yalign = 0;
		v.PackStart(l);
		
		l = new Label(explanation);
		l.LineWrap = true;
		l.Selectable = false;
		l.Xalign = 0;
		l.Yalign = 0;
		v.PackEnd(l);
		
		h.PackEnd(v);
		h.ShowAll();
		this.VBox.Add(h);
		
		this.AddButton(Stock.Ok, ResponseType.Ok);
		this.Modal = true;
	}		
}