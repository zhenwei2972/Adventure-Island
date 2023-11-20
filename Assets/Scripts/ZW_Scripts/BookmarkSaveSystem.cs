using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class BookmarkSaveSystem 
{

    //static save folder string.
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

	public static void Init()
	{
		//check if save folder path exists
		if (!Directory.Exists(SAVE_FOLDER))
		{
			Directory.CreateDirectory(SAVE_FOLDER);
		}

		
	}
	public static void Save(string saveString)
	{
		File.WriteAllText(SAVE_FOLDER + "Save", saveString);
	}

	public static string Load()
	{
		if (File.Exists(SAVE_FOLDER + "Save"))
		{
			string saveString = File.ReadAllText(SAVE_FOLDER + "Save");
			return saveString;
		
		}
		return null;
	}

	//
}
