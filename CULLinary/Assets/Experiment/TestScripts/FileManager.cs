using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class FileManager
{
    public static bool WriteToFile(string a_FileName, string a_FileContents)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);
        Debug.Log(fullPath);

        try
        {
            File.WriteAllText(fullPath, a_FileContents);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to write to {fullPath} with exception {e}");
            return false;
        }
    }

    public static bool LoadFromFile(string a_FileName, out string result, bool isAbsolutePath=false)
    {
        string fullPath;
        if (isAbsolutePath)
        {
            fullPath = a_FileName;
            
        }
        else
        {
            fullPath = Path.Combine(Application.persistentDataPath, a_FileName);
        }

        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to read from {fullPath} with exception {e}");
            result = "";
            return false;
        }
    }
}