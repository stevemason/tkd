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

// created on 22/06/2005 at 15:43
using System.IO;
using System;

////////////////////////////////////////////////////////////////////////////////////
// saves & loads config data to a file.                                            /
//                                                                                 /
// We need to take into account that data needs to be saved in different places    /
// depending on the OS.                                                            /
// In Linux, the data should be saved as '.nameofapp' in the user's home folder    /
// In any other environment, the location of the file will default to the          /
// location of the program executable, but with an option to specify an alternate  /
// location.                                                                       /
//                                                                                 /
// So the order of events should be:                                               /
// 1 - check for the existance of configpath.ini in the program's directory.       /
//     If it exists, use the location within, if possible.                         /
// 2 - check to see if the program is running on linux                             /
// 	   If so, use the location ~/.nameofapp                                        /
// 3 - final option, try the program's location                                    /
//                                                                                 /
//                                                                                 /
//To set the data to be saved, use the SetData method. To access the data read     /
//in from a file, access the variable Config.stringConfigData directly             /
////////////////////////////////////////////////////////////////////////////////////
//public variables:                                                                /
//  string[] stringConfigData    to access data read from config file              /
////////////////////////////////////////////////////////////////////////////////////
//public methods:                                                                  /
//  constructor                                                                    /
//    string nameofapp           used to create filename for config file           /
//                                                                                 /
//  load [bool]                  load config data from file.                       /
//                               Returns true if successful                        /
//                                                                                 /
//  save [bool]                  save config data to file                          /
//                               Returns true if successful                        /
//                                                                                 /
//  SetData [void]               set data to be saved to config file               /
//    string[] data2set          data to be saved to file                          /
////////////////////////////////////////////////////////////////////////////////////

public class Config
{
	public string[] stringConfigData; //stores the data to be read / saved
	private string stringConfigPath; //the path of the config file

	public Config(string nameofapp)
	{
		//determine the location of the config file
		
		// 1 - check for the existance of configpath.ini in the program's directory.
		//     If it exists, use the location within, if possible.
		FileInfo fileinfoConfigPath = new FileInfo(@"configpath.ini");
		if (fileinfoConfigPath.Exists == true)
		{
			//read stringConfigPath from configpath.ini
			StreamReader streamreaderConfigPath = fileinfoConfigPath.OpenText();
			stringConfigPath = streamreaderConfigPath.ReadLine();
			streamreaderConfigPath.Close();	
		}
		else
		{
			// 2 - check to see if the program is running on linux
			// 	   If so, use the location ~/.nameofapp
			
			// NOTE: the value of 128 refers to any *ix os, not Linux specifically
			if ((int) Environment.OSVersion.Platform == 128)
			{
				//set stringConfigPath to ~/.nameofapp
				stringConfigPath = System.Environment.GetEnvironmentVariable("HOME") + "/." + nameofapp;
			}
			else
			{
				// 3 - final option, try the program's location
				stringConfigPath = nameofapp + ".ini";
			}
		}		
	} 
	
	// Load the config data from a file, if possible
	// If data is loaded, return 'true', otherwise 'false'
	public bool Load()
	{
		try{
			FileInfo fileinfoConfigData = new FileInfo(@stringConfigPath);
		
			//get number of entries
			StreamReader streamreaderLoadConfig = fileinfoConfigData.OpenText();
			string text;
			int size = 0;
			do
			{
				text = streamreaderLoadConfig.ReadLine();
				size += 1;
			}while (text != null);
			streamreaderLoadConfig.Close();
			
			stringConfigData = new string[size];
			// now read the data into stringConfigData[]
			
			StreamReader streamreaderLoadData = fileinfoConfigData.OpenText();
			for (int i = 0; i<size; i++)
			{
				stringConfigData[i] = streamreaderLoadData.ReadLine();
			}
			streamreaderLoadData.Close();
			return true;
			}
			
			catch (Exception Exc)
			{
				Console.Error.WriteLine("Error: "+Exc.Message);
				return false;
			}	
		}
	
	// Save the config data to a file, if possible.
	// If data is saved, return 'true', otherwise 'false'
	public bool Save()
	{
		try
		{
			StreamWriter streamwriterSaveConfig = new StreamWriter(File.Open(@stringConfigPath, FileMode.OpenOrCreate));
			foreach(string line in stringConfigData)
			{
				streamwriterSaveConfig.WriteLine(line);
			}
			streamwriterSaveConfig.Close();
			return true;
		}
		
		catch (Exception Exc)
		{
			Console.Error.WriteLine("Error: "+Exc.Message);
			return false;
		}
	}
	
	public void SetData(string[] data2set)
	{
		stringConfigData = data2set;
	}
}
