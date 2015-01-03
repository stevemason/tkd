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

// created on 18/06/2005 at 13:43
using System.IO;
using System;

//////////////////////////////////////////////////////////////////////////////////
// Reads a Tab Separated Values file and returns an array containing these values/
//////////////////////////////////////////////////////////////////////////////////
// Constructor takes filepath (string TSVfile) as a parameter                    /
//////////////////////////////////////////////////////////////////////////////////
// Public variables:                                                             /
//   int fields            the number of fields per line                         /
//   int lines             the number of lines (records)                         /
//   string[,] FileArray   array for storing file data                           /
//////////////////////////////////////////////////////////////////////////////////

public class ReadTSV
{
	public int fields; //number of fields per line
	public int lines;  //number of lines
	public string[,] FileArray; //array for storing file data
	private string splitpart;
	private string base_directory = System.AppDomain.CurrentDomain.BaseDirectory;
	public ReadTSV(string TSVfile)
	{
		try
		{
			TSVfile = Path.Combine(base_directory, TSVfile);
			FileInfo fileinfoSourceFile = new FileInfo(@TSVfile);
		
			/// First of all read data from the file to establish number of fields and lines
		
			StreamReader streamreaderGetInfo = fileinfoSourceFile.OpenText();
			string text;
		
			// Read in the 1st line to find number of fields
		
			text = streamreaderGetInfo.ReadLine();
			string[] splitGetInfo = text.Split(new char[] {'\t'});
			fields = splitGetInfo.Length;

			// Read in the rest of the file to establish number of lines
			do
			{
				text = streamreaderGetInfo.ReadLine();
				lines += 1;
			} while (text != null);
		
			streamreaderGetInfo.Close();
		
			//Array to read the file into
			FileArray = new string[lines,fields];
		
			//Now read in the data from the file
		
			StreamReader streamreaderReadFileData = fileinfoSourceFile.OpenText();
		
			for (int i=0; i<(lines);i++)
			{
				text = streamreaderReadFileData.ReadLine();
				string[] split = text.Split(new char[] {'\t'});
				for (int j=0; j<(fields);j++)
				{
					//remove unneeded quotes - if open quote present, assume end quote present
					splitpart = split[j];
					if (splitpart.Substring(0,1) == "\"")
					{
						splitpart = splitpart.Substring(1, splitpart.Length-2);
						
					}
					FileArray[i,j] = splitpart;
				}
			} 
			
			streamreaderReadFileData.Close();
		}
		catch (Exception Exc)
		{
			Console.Error.WriteLine("Error: "+Exc.Message);
		}			
	}		
}