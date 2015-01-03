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

// created on 17/06/2005 at 21:04

using System;

/////////////////////////////////////////////////////////////////////////////////////
//This class manages the core functionality of the test. The test is initialised    /
//by calling the Initialise method. The test is then managed by calling the methods /
//below.                                                                            /
/////////////////////////////////////////////////////////////////////////////////////
//public methods:                                                                   /
//                                                                                  /
//  Initialise [void]    resets the test according to the following parameters      /
//    int grade          which kup are you studying for?                            /
//    bool onlynew       only test new theory for this grading?                     /
//    bool[] topix       which topics to include (true = include)                   /
//    bool pango         add pango text formatting tags                             /
//    bool limit         limit the number of questions?                             /
//                                                                                  /
//  GetGrade [int]       returns current grade                                      /
//                                                                                  /
//  Question [string]    returns current question                                   /
//                                                                                  /
//  Answer [string]      returns current answer                                     /
//                                                                                  /
//  Correct [void]       removes current question from database and sets up new     /
//                       question                                                   /
//                                                                                  /
//  Incorrect [void]     sets up new question without deleting current question     /
//                       so that it will be asked again later                       /
//                                                                                  /
//  QuestionsRemaining [int]   returns the number of questions remaining            /
/////////////////////////////////////////////////////////////////////////////////////
//required:                                                                         /
//  ReadTSV class        reads in the data from the tkd.txt file                    /
//  tkd.txt file         file containing question data                              /
/////////////////////////////////////////////////////////////////////////////////////

public class Test
{
	private int grade;	//the current grade being tested
	private bool[] topic; //topics to include in the test
	private int numberofquestions;
	private int questionnumber;
	private ReadTSV TestFile = new ReadTSV("tkd.txt");
	private string[,] TestQuestions;
	private int currentquestion;
	public void Initialise(int grade, bool onlynew, bool[] topix, bool pango, int limit)
	{
		this.grade = grade;
		this.topic = topix;
		
		//Now use this information to generate a relevant question database
		
		//First calculate the number of questions
		numberofquestions = 0;
		
		for (int i = 0; i < TestFile.lines; i++)
			
		{
			//The numerical values from the data file are actually read in as strings
			//So these need to be converted before they can be treated as numbers
			int currentgrade = Convert.ToInt32(TestFile.FileArray[i,1]);
			int	currenttopic = Convert.ToInt32(TestFile.FileArray[i,0]);
			//Checking the current question from file:
			//Is the 'kup' of the question >= the required kup?
			//If 'only new questions' has been selected, 'kup' must be = required kup
			
			if (((currentgrade >= grade) & (onlynew == false)) | ((currentgrade == grade) & (onlynew == true)))
			{
				//topic 0 data is needed for topic 1 questions also.
				//for Korean Counting, both Eng > Kor and Kor > Eng are included
				if (((currenttopic == 0) & (topic[1] == true)) | ((currenttopic == 7) & (topic[7] == true)))
				{
					numberofquestions += 1;
				}	 
		
				if (topic[currenttopic] == true)
				{
					numberofquestions += 1;
				}
			}
		}
		
		TestQuestions = new string[numberofquestions,2];
		questionnumber = -1;
				
		for (int i = 0; i < TestFile.lines; i++)
		{
			//The numerical values from the data file are actually read in as strings
			//So these need to be converted before they can be treated as numbers
			int currentgrade = Convert.ToInt32(TestFile.FileArray[i,1]);
			int	currenttopic = Convert.ToInt32(TestFile.FileArray[i,0]);
		
			//Checking the current question from file:
			//Is the 'kup' of the question >= the required kup?
			//If 'only new questions' has been selected, 'kup' must be = required kup
			
			if (((currentgrade >= grade) & (onlynew == false)) | ((currentgrade == grade) & (onlynew == true)))
			{
				//topic 0 data is needed for topic 1 questions also.
				
				if (((currenttopic == 0) & (topic[1] == true)) | ((currenttopic == 7) & (topic[7] == true)))
				{
					questionnumber += 1;
					if (pango == true)
					{
						TestQuestions[questionnumber,0] = "Translate the following into English: <span foreground=\"red\">" + TestFile.FileArray[i,3]+"</span>";
					}
					else
					{
						TestQuestions[questionnumber,0] = "Translate the following into English: " + TestFile.FileArray[i,3];
					}
					TestQuestions[questionnumber,1] = TestFile.FileArray[i,2];
				}	 
				
	
				if (topic[currenttopic] == true)
				{
					questionnumber += 1;
					TestQuestions[questionnumber,0] = TestFile.FileArray[i,2];
					TestQuestions[questionnumber,1] = TestFile.FileArray[i,3];
				}
				
				//add extra text to topics 0 and 6
				if (((currenttopic == 0) & (topic[0] == true)) | ((currenttopic == 7) & (topic[7] == true)))
				{
					if (pango == true)
					{
						TestQuestions[questionnumber,0] = "Translate the following into Korean: <span foreground=\"blue\">" + TestQuestions[questionnumber,0]+"</span>";
					}
					else
					{
						TestQuestions[questionnumber,0] = "Translate the following into Korean: " + TestQuestions[questionnumber,0];
					}
				}
				
				if ((currenttopic == 6) & (topic[6] == true))
				{
					if (pango == true)
					{
						TestQuestions[questionnumber,0] = "Describe the following stance: <span foreground=\"orange\">" + TestQuestions[questionnumber,0]+"</span>";
					}
					else
					{
						TestQuestions[questionnumber,0] = "Describe the following stance: " + TestQuestions[questionnumber,0];
					}
				}
			}
		}
		
		NewQuestion();
		
		//If the number of questions asked is going to be limited, we need to delete questions at random to bring it down to the limit
		if (limit != -1)
		{
			while (numberofquestions > limit)
			{
				Correct(); //delete a question from the database (then choose another at random)
			}
		}
		
		
	}
	
	public int GetGrade()
	{
		return grade;
	}
	
	public string Question()
	{
		//returns the current question
		
		return TestQuestions[currentquestion,0];
	}
	
	public string Answer()
	{
		//returns the answer to the current question
		return TestQuestions[currentquestion,1];
	}

	public void Correct()
	{
		//correct answer - remove current question from database
		//replace it with the last question in database
		//then reduce the number of questions
		//this is fine to do because the actual question order is not important and
		//does not need to be preserved
			
		TestQuestions[currentquestion,0] = TestQuestions[numberofquestions-1,0];
		TestQuestions[currentquestion,1] = TestQuestions[numberofquestions-1,1];
		numberofquestions -= 1;
		
		NewQuestion();
	}
	
	public void Incorrect()
	{
		//Move on to the next question without removing the current one from the database so that it'll be asked again
		NewQuestion();
	}	
	
	private void NewQuestion()
	{
		Random rand = new Random();
		currentquestion = rand.Next(numberofquestions); 
	}
	
	public int QuestionsRemaining()
	{
		return numberofquestions;
	}	
}