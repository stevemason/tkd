//	"Taekwon-Do Theory Assistant" by Steven Mason

//	Copyright (C) 2007 Steven Mason

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

using System;
using System.IO;
using Gtk;

////////////////////////////////////////////////////////////////////////////////
//This class deals with the GUI side of test.                                  /
//The core test functionality is provided by the Test class.                   /
////////////////////////////////////////////////////////////////////////////////
//required:                                                                    /
//  Test class                core functionality of the test                   /
//  Config class              load/save configuration data                     /
//  TestDialog class          "no questions in test" dialog                    /
//  EndOfTestDialog class     "end of test" dialog                             /
//  YesNoDialog class         "are you sure you want to stop the test?" dialog /
//  icon.png                  icon for all windows and dialog boxes            /
//  tkd.png                   image for main options window                    /
////////////////////////////////////////////////////////////////////////////////

public class MainWindow : Window {
	// Instead of creating a child window for the test, the main window is also being used for the test.
	// All of the widgets for the "main window" are stored in objVBox
	// All of the widgets for the "test window" are stored in objVBoxTest
	// 
	// objVBoxDaddy is a container for either objVBox or objVBoxTest

	// The blocks of widget code to do with the test window are kept separate and marked as

	////////////////////////
	//TEST

	//END TEST
	////////////////////////

	// All other widget code should be to do with the main window 	

	const int subjects = 8; // number of different topics available in test - change this value if more topics are added / removed
	
	private string base_directory = System.AppDomain.CurrentDomain.BaseDirectory;
	private string image_path;
	private string icon_path;
	
	Config configTKD = new Config("tkd"); //object for loading/saving config data
	CheckButton[] objCheckButtonTopic = new CheckButton[subjects];
	string[] belts = new string[]{"Black Belt (1st Dan) Grading", "Black Tag (1st Kup) Grading", "Red Belt (2nd Kup) Grading", "Red Tag (3rd Kup) Grading", "Blue Belt (4th Kup) Grading", "Blue Tag (5th Kup) Grading", "Green Belt (6th Kup) Grading", "Green Tag (7th Kup) Grading", "Yellow Belt (8th Kup) Grading", "Yellow Tag (9th Kup) Grading"};
	string[] limits = new string[]{"Max. 10 exercises", "Max. 20 exercises", "Max. 30 exercises", "Max. 40 exercises", "Max. 50 exercises", "Max. 100 exercises", "Max. 200 exercises", "No limit to test length"};
	//the belt you're going for. 9=yellow tag, 0=black
	int belt;
	
	//the max question limit, 0=unlimited
	int intLimit;
	
	//beltcolors[grade,(0=belt, 1=tag)]
	Gdk.Color[,] beltcolour = new Gdk.Color[10,2];
	
	Combo objComboBelt = new Combo();
	Combo objComboLimit = new Combo();
	
	EventBox objEventBoxBeltTop = new EventBox();
	EventBox objEventBoxBeltMiddle = new EventBox();
	EventBox objEventBoxBeltBottom = new EventBox();
	
	Button objButtonStart = new Button("_Start Test");
	CheckButton objCheckButtonNew = new CheckButton("_Only include new theory");
	
	Test objTest = new Test();
	Box objVBox = new VBox(); //the main container
	Box objVBoxDaddy = new VBox();
	
	///////////////////////////////////////////////////////////////////////
	// TEST (These widgets are for the test screen)
	Label objLabelPadding = new Label("                                                ");
	Label objLabelQuestion = new Label();
	Label objLabelAnswer = new Label();
	Box objVBoxTest = new VBox();
	Button objButtonCorrect = new Button("MY ANSWER WAS CO_RRECT");
	Button objButtonIncorrect = new Button("MY ANSWER WAS INCORREC_T");
	Button objButtonDisplayAnswer = new Button("DISPLAY ANSWE_R");
	Label objLabelNumberRemaining = new Label(); //white box showing number of questions remaining
	// END TEST
	///////////////////////////////////////////////////////////////////////
	
	public MainWindow () : base ("Taekwon-Do Theory Assistant 1.2.2")
	{
		Console.WriteLine("Taekwon-Do Theory Assistant version 1.2.2, Copyright (c) 2007 Steven Mason");
		Console.WriteLine("Taekwon-Do Theory Assistant comes with ABSOLUTELY NO WARRANTY.");
		Console.WriteLine("This is free software, and you are welcome to redistribute it,");
		Console.WriteLine("under certain conditions. See gpl.txt for details.");
		
		Box objHBoxTop = new HBox(); //the top part

		Box objVBoxOptions = new VBox(); //container for checkboxes
		Box objVBoxBigOptions = new VBox(); //container for checkbox container + buttons
		
		Box objVBoxSelect = new VBox(); //containter for select/deselect all
		Box objVBoxPic = new VBox(); //container for image & belt selection
		
		Box objVBoxBelt = new VBox(); //belt
		
		image_path = System.IO.Path.Combine(base_directory, "tkd.png");
		
		//IMAGE
		Image objImageTKD = new Image(image_path);
		objImageTKD.SetPadding(20,20);		
		objVBoxPic.Add(objImageTKD);
		
		//BELT SELECTION COMBO
		objComboBelt.PopdownStrings = belts;
		objComboBelt.Entry.Editable = false;
		objVBoxPic.Add(objComboBelt);
		
		objHBoxTop.Add(objVBoxPic);
		
		//TOPIC CHECKBOXES
		string[] topic = new string[subjects]{"_English > Korean", "_Korean > English", "_Patterns","_Belts", "TKD _History", "_Tenets","St_ances", "Korean Cou_nting"};
		for (int i = 0; i < subjects; i++)
		{
		objCheckButtonTopic[i] = new CheckButton(topic[i]);
		objVBoxOptions.Add(objCheckButtonTopic[i]);
		}
		
		//TEST TOPIC FRAME
		Frame objFrameTopic = new Frame("Test Topics");
		objFrameTopic.Add(objVBoxOptions);
		objVBoxBigOptions.Add(objFrameTopic);
		
		//SELECT ALL + DESELECT ALL + NEW BUTTONS + LIMIT COMBO
		Button objButtonSelectAll = new Button("Sele_ct All");
		Button objButtonDeselectAll = new Button("_Deselect All");
		
		objVBoxSelect.Add(objButtonSelectAll);
		objVBoxSelect.Add(objButtonDeselectAll);
		
		objVBoxSelect.Add(objCheckButtonNew);
		
		objComboLimit.PopdownStrings = limits;
		objComboLimit.Entry.Text = "Select test length ->";
		objComboLimit.Entry.Editable = false;
		objVBoxSelect.Add(objComboLimit);
		
		objVBoxBigOptions.Add(objVBoxSelect);
		
		//START TEST
		objVBoxBigOptions.Add(objButtonStart);
		
		objVBoxBigOptions.Spacing = 5;
		
		objHBoxTop.Add(objVBoxBigOptions);
		objHBoxTop.Spacing = 30;
		objVBox.Add(objHBoxTop);
		
		// BELT
        
		objVBoxBelt.Add(objEventBoxBeltTop);
		objVBoxBelt.Add(objEventBoxBeltMiddle);
		objVBoxBelt.Add(objEventBoxBeltBottom);
		objVBoxBelt.HeightRequest = 40;
				
		objVBox.Add(objVBoxBelt);
		
		objVBox.BorderWidth = 5;
		
		////////////////////////////////////////////////////////////////
		//TEST (These widgets are for the test screen)
		
		Box objVBoxTestQuestionAnswer = new VBox(); //container for question and answer
	
		Box objHBoxTestTop = new HBox(); //container for stop and questions remaining
	
		//STOP TEST
		Button objButtonStop = new Button("_STOP TEST");
		objHBoxTestTop.Add(objButtonStop);
	
		//QUESTIONS REMAINING
		objHBoxTestTop.Add(objLabelPadding);

		Alignment objAlignmentRemain = new Alignment(1,0,0,1); //top-align + don't expand horizontally
		objAlignmentRemain.Add(objLabelNumberRemaining);
		objHBoxTestTop.Add(objAlignmentRemain);
	
		//QUESTION
		objLabelQuestion.LineWrap = true;
		objLabelQuestion.HeightRequest=60;
		objVBoxTestQuestionAnswer.Add(objLabelQuestion);
		
		//ANSWER
		objLabelAnswer.LineWrap = true;
		objLabelAnswer.HeightRequest=270;
		objVBoxTestQuestionAnswer.Add(objLabelAnswer);
	
		Box objHBoxTestBottom = new HBox(); //container for correct/incorrect/display buttons
	
		//CORRECT BUTTON
		objButtonCorrect.HeightRequest = 50;
		objHBoxTestBottom.Add(objButtonCorrect);
	
		//DISPLAY ANSWER BUTTON
		objButtonDisplayAnswer.HeightRequest = 50;
		objHBoxTestBottom.Add(objButtonDisplayAnswer);
		
		//INCORRECT BUTTON
		objButtonIncorrect.HeightRequest = 50;
		objHBoxTestBottom.Add(objButtonIncorrect);
			
		objVBoxTest.Add(objHBoxTestTop);
		objVBoxTest.Add(objVBoxTestQuestionAnswer);
		objVBoxTest.Add(objHBoxTestBottom);
		objVBoxTest.BorderWidth = 5;
		//END TEST
		/////////////////////////////////////////////////////////////
		
		//GTK events for main and test "windows"		
		objButtonStart.Clicked += new EventHandler(objButtonStart_Clicked);
		objButtonSelectAll.Clicked += new EventHandler(objButtonSelectAll_Clicked);
		objButtonDeselectAll.Clicked += new EventHandler(objButtonDeselectAll_Clicked);
		objComboBelt.Entry.Changed += new EventHandler (objComboBelt_Changed);
		objComboLimit.Entry.Changed += new EventHandler (objComboLimit_Changed);
		objButtonStop.Clicked += new EventHandler (objButtonStop_Clicked);
		objButtonDisplayAnswer.Clicked += new EventHandler (objButtonDisplayAnswer_Clicked);
		objButtonCorrect.Clicked += new EventHandler (objButtonCorrect_Clicked);
		objButtonIncorrect.Clicked += new EventHandler (objButtonIncorrect_Clicked);
		
		this.DeleteEvent += new DeleteEventHandler (OnMainWindowDelete);
		this.KeyPressEvent += new KeyPressEventHandler (keypressevent); //For keyboard shortcuts without the need to press alt
		
		//set initial values of topic checkboxes & belt	
		
		//read data from config file
		
		if (configTKD.Load() == true) //success
		{			
			for(int i = 0; i<subjects;i++)
			{
				objCheckButtonTopic[i].Active = Convert.ToBoolean(configTKD.stringConfigData[i+1]);
			}
			objCheckButtonNew.Active = Convert.ToBoolean(configTKD.stringConfigData[subjects+1]);
			belt = Convert.ToInt32(configTKD.stringConfigData[subjects+2]);
			
			// If an upgrade has just been made from v1.0, there will not be a line in the config file for intLimit which means that stringConfigData[10] won't exist. This will cause an error. If this happens, the error is trapped and a default value is set.
			try
			{
				intLimit = Convert.ToInt32(configTKD.stringConfigData[subjects+3]);
			}
			catch
			{
				intLimit = 0; //value not read from file, probably due to upgrade from 1.0
			}
		}
		else
		{
			// there was a problem reading the config data, so use these values by default
			for (int i=0; i<subjects; i++)
			{
				objCheckButtonTopic[i].Active = true;
			}
			belt = 9; //yellow tag
			intLimit=0; //value not read from file
		}
		
		//Add ToolTips
		
		//Main Window
		Tooltips tooltips = new Tooltips();
		tooltips.SetTip(objButtonStart,"Once you have selected the belt that you are going for and the topics that you would like to be tested on, click here to start the test",null);
		tooltips.SetTip(objButtonSelectAll,"Selects all of the above test topics",null);
		tooltips.SetTip(objButtonDeselectAll,"Clears all of the above tick boxes",null);
		tooltips.SetTip(objCheckButtonNew,"Select this tick box if you do not want to be tested on theory that you have already learned for previous gradings",null);
		tooltips.SetTip(objCheckButtonTopic[0],"You will be shown English terminology and you should translate into Korean",null);
		tooltips.SetTip(objCheckButtonTopic[1],"You will be shown Korean terminology and you should translate into English",null);
		tooltips.SetTip(objCheckButtonTopic[2],"Questions will be asked about the meaning of the pattern and the number of moves",null);
		tooltips.SetTip(objCheckButtonTopic[3],"Questions will be asked about the significance of the different belts",null);
		tooltips.SetTip(objCheckButtonTopic[4],"General knowledge questions will be asked on the history of Taekwon-Do",null);
		tooltips.SetTip(objCheckButtonTopic[5],"Questions will be asked on the tenets of Taekwon-Do",null);
		tooltips.SetTip(objCheckButtonTopic[6],"You will be asked to accurately describe Taekwon-Do stances",null);
		tooltips.SetTip(objCheckButtonTopic[7],"You will be tested on the numbers 1 to 10 in Korean",null);
		
		//Test Window
		tooltips.SetTip(objButtonStop,"Click here to exit the test and return to the options window",null);
		
		/////////////
		
		objButtonStart.HasFocus = true;
		
		objComboBelt.Entry.Text = belts[belt];
		
		SetComboLimitText();
		
		SetBeltColour();
		objVBox.Spacing = 5;
		//Display the Main (options) window
		objVBoxDaddy.Add(objVBox);				
		this.Add(objVBoxDaddy);
		icon_path = System.IO.Path.Combine(base_directory, "icon.png");
		this.SetIconFromFile(icon_path);
		this.ShowAll ();
	}
	
	void SetComboLimitText()
	{
		//If this is not the first time that v>=1.1 has been run, set the combobox to display the current limit
		if (intLimit !=0)
		{
			switch (intLimit)
			{
				case -1:
					objComboLimit.Entry.Text = limits[7];
					break;
				case 10:
					objComboLimit.Entry.Text = limits[0];
					break;
				case 20:
					objComboLimit.Entry.Text = limits[1];
					break;
				case 30:
					objComboLimit.Entry.Text = limits[2];
					break;
				case 40:
					objComboLimit.Entry.Text = limits[3];
					break;
				case 50:
					objComboLimit.Entry.Text = limits[4];
					break;
				case 100:
					objComboLimit.Entry.Text = limits[5];
					break;
				case 200:
					objComboLimit.Entry.Text = limits[6];
					break;
			}
		}
		else
		{
			intLimit = -1; // don't change the combobox display (leave the default message), but set intLimit to the default value of 0 (unlimited)
		}
	}
		
	void OnMainWindowDelete (object o, DeleteEventArgs args)
	{
		//prepare data to save
		string[] stringStuff2Write = new string[12];
		stringStuff2Write[0] = "This file has been generated by the Taekwon-do Theory Assistant - please do not edit it by hand";
		for(int i = 0; i<subjects;i++)
		{
			stringStuff2Write[i+1] = Convert.ToString(objCheckButtonTopic[i].Active);
		}
		stringStuff2Write[subjects+1] = Convert.ToString(objCheckButtonNew.Active);
		stringStuff2Write[subjects+2] = Convert.ToString(belt);
		stringStuff2Write[subjects+3] = Convert.ToString(intLimit);
		
		//try to save config data before exiting. If the save doesn't work it's
		//not that big a deal...

		configTKD.SetData(stringStuff2Write);
		configTKD.Save();
		
		Application.Quit ();
	}
	
	void objButtonSelectAll_Clicked (object o, EventArgs args)
	{
		// Select all test topics
        foreach (CheckButton check in objCheckButtonTopic)
        {
        	check.Active = true;
        }
	}
	
	void objButtonDeselectAll_Clicked (object o, EventArgs args)
	{
		// Deselect all test topics        
        foreach (CheckButton check in objCheckButtonTopic)
        {
        	check.Active = false;
        }
	}

	void objButtonStop_Clicked (object o, EventArgs args)
	{
		YesNoDialog d = new YesNoDialog("Stop - are you sure?","Are you sure that you want to stop the test?","It's not too late to turn back.");
		ResponseType resp = (ResponseType) d.Run();
		d.Destroy();
		if (resp == ResponseType.Yes)
		{
			//Stop the test and return to the Main (options) menu
			//Hide the test and display the options
			SwitchToMain();
		}
	}
	
	void objButtonCorrect_Clicked (object o, EventArgs args)
	{
		if (objTest.QuestionsRemaining() == 1)
		{
			EndOfTestDialog d = new EndOfTestDialog("Test completed","Test completed","Congratulations!");
			d.Run();
			d.Destroy();
			
			//Stop the test and return to the Main (options) menu
			//Hide the test and display the options
			SwitchToMain();
		}
		else
		{ 
		objTest.Correct();
		NextQuestion();
		}
	}
	
	void objButtonIncorrect_Clicked (object o, EventArgs args)
	{
		objTest.Incorrect();
		NextQuestion();
	}
	
	void NextQuestion ()
	{
		// Update the display with the new question
		objLabelQuestion.Text = "<span weight=\"bold\" size=\"larger\">" + objTest.Question() + "</span>";
		objLabelQuestion.UseMarkup = true;
		objLabelAnswer.Text = "";
		objLabelNumberRemaining.Text = "<span background=\"white\">Exercises remaining:  <span foreground=\"red\">" + Convert.ToString(objTest.QuestionsRemaining()-1)+"</span></span>";
		objLabelNumberRemaining.UseMarkup = true;
		objButtonDisplayAnswer.Show();
		objButtonDisplayAnswer.HasFocus = true;
		objButtonCorrect.Hide();
		objButtonIncorrect.Hide();
	}
	
	void objButtonDisplayAnswer_Clicked (object o, EventArgs args)
	{
		objLabelAnswer.Text = "<span weight=\"bold\" size=\"larger\">" + objTest.Answer() + "</span>";
		objLabelAnswer.UseMarkup = true;
		objButtonDisplayAnswer.Hide();
		objButtonCorrect.Show();
		objButtonIncorrect.Show();
		objButtonCorrect.HasFocus = true;
	}
	
	void SetBeltColour()
	{
		//define belt colours
		Gdk.Color beltwhite = new Gdk.Color(255,255,255);
		Gdk.Color beltyellow = new Gdk.Color(251,235,32);
		Gdk.Color beltgreen = new Gdk.Color(0,138,69);
		Gdk.Color beltblue = new Gdk.Color(0,0,176);
		Gdk.Color beltred = new Gdk.Color(255,0,0);
		Gdk.Color beltblack = new Gdk.Color(0,0,0);
				
		//yellow tag
		beltcolour[9,0] = beltwhite;
		beltcolour[9,1] = beltyellow;
		//yellow
		beltcolour[8,0] = beltyellow;
		beltcolour[8,1] = beltyellow;
		//green tag
		beltcolour[7,0] = beltyellow;
		beltcolour[7,1] = beltgreen;
		//green
		beltcolour[6,0] = beltgreen;
		beltcolour[6,1] = beltgreen;
		//blue tag
		beltcolour[5,0] = beltgreen;
		beltcolour[5,1] = beltblue;
		//blue
		beltcolour[4,0] = beltblue;
		beltcolour[4,1] = beltblue;
		//red tag
		beltcolour[3,0] = beltblue;
		beltcolour[3,1] = beltred;
		//red
		beltcolour[2,0] = beltred;
		beltcolour[2,1] = beltred;
		//black tag
		beltcolour[1,0] = beltred;
		beltcolour[1,1] = beltblack;
		//black
		beltcolour[0,0] = beltblack;
		beltcolour[0,1] = beltblack;
	
		objEventBoxBeltTop.ModifyBg(Gtk.StateType.Normal, beltcolour[belt,0]);
		objEventBoxBeltMiddle.ModifyBg(Gtk.StateType.Normal, beltcolour[belt,1]);		
		objEventBoxBeltBottom.ModifyBg(Gtk.StateType.Normal, beltcolour[belt,0]);
	}
	
	void objComboBelt_Changed (object o, EventArgs args)
	{		
		for (int i = 0; i < 10; i++)
		{
			if (objComboBelt.Entry.Text == belts[i])
			{
				belt = i;				
			}
		}
		SetBeltColour();		
	}
	
	void objComboLimit_Changed (object o, EventArgs args)
	{
		switch (objComboLimit.Entry.Text)
		{
			case "Max. 10 exercises":
				intLimit = 10;
				break;
			case "Max. 20 exercises":
				intLimit = 20;
				break;
			case "Max. 30 exercises":
				intLimit = 30;
				break;
			case "Max. 40 exercises":
				intLimit = 40;
				break;
			case "Max. 50 exercises":
				intLimit = 50;
				break;
			case "Max. 100 exercises":
				intLimit = 100;
				break;
			case "Max. 200 exercises":
				intLimit = 200;
				break;
			case "No limit to test length":
				intLimit = -1;
				break;
		}
	}
	
	void objButtonStart_Clicked (object o, EventArgs args)
	{
		//////////////////////////////////////////////////
		//Initialise Test
		//////////////////////////////////////////////////
		// Create the topics array which will contain the values of the checkboxes
		// to be passed to objTest.Initialise
		bool[] topics = new bool[subjects];
		for (int i=0; i<subjects; i++)
		{
			topics[i] = objCheckButtonTopic[i].Active;
		}
		// Initialise	
		objTest.Initialise(belt, objCheckButtonNew.Active, topics, true, intLimit);
	
		//Check to see if there are actually any questions in the test
		if (objTest.QuestionsRemaining() == 0 )
		{
			//Oh dear, no questions
			TestDialog d = new TestDialog("No questions in test","There are no questions in the test, based on your current selections.","Try selecting more question topics.");
			d.Run();
			d.Destroy();
		}
		else
		{		
			//YAY - we have some questions - let's go!
			//Hide the Main (options) stuff and display the test
			objVBoxDaddy.Remove(objVBox);
			objVBoxDaddy.Add(objVBoxTest);
			this.ShowAll();
			NextQuestion(); // Update display with first question
		}	
	}
	
	void SwitchToMain()
	{
		//Hide the test and display the options
		objVBoxDaddy.Remove(objVBoxTest);
		objVBoxDaddy.Add(objVBox);
		objButtonStart.HasFocus = true;
		this.ShowAll();
	}
	
	
	//Keyboard shortcuts without the need to press alt
	//
	//Most shortcuts in the app still require alt, but the three main buttons
	//During the test (display answer, correct, incorrect) will have keyboard shortcuts 
	//that work without alt.
	//
	//Use button visibility to check that the shortcut is being used at an appropriate time
	//ie to make sure that the objButtonCorrect_Clicked isn't called when the options window is visible
	void keypressevent(object o, KeyPressEventArgs args)
	{
		switch (args.Event.Key)
		{
			case Gdk.Key.r:
			case Gdk.Key.R:
				if (objButtonDisplayAnswer.Visible == true)
				{
					//display answer
					objButtonDisplayAnswer_Clicked(null, null);
				}
				else
				{
					if (objButtonCorrect.Visible == true)
					{
						//answer correct
						objButtonCorrect_Clicked(null,null);
					}
				}
				break;
			case Gdk.Key.t:
			case Gdk.Key.T:
				if (objButtonIncorrect.Visible == true)
				{
					//answer incorrect
					objButtonIncorrect_Clicked(null,null);
				}
				break;
			case Gdk.Key.Escape:
				OnMainWindowDelete(null,null);
				break;
		}
	}
}