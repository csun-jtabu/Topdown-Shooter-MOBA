using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;

//public class GenerateMap : MonoBehaviour
public class GenerateMap
{
    public string generatedOutput;

    public void Start()
    {
        RunPython(); 
    }

    void RunPython()
    {
        // Python script path
        string scriptPath = Application.dataPath + "/Scripts/LevelGen/LevelPatternGeneration.py"; 

        // this will be the command line arguments
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "python",  // python file
            Arguments = "\"" + scriptPath + "\"", // the script we are running
            UseShellExecute = false,  // this executes it without opening the shell
            RedirectStandardOutput = true, // we redirect anything from the shell
            RedirectStandardError = true,
            CreateNoWindow = true,  // this doesn't open a new window when executing
            WorkingDirectory = Application.dataPath + "/Scripts/LevelGen/"  // this is the path we are working in
        };

        // basically starts a new process using the command line
        using (Process process = new Process { StartInfo = psi })
        {   
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            //process.Start(); // starts the command line process
            //
            //// starts outputting
            //process.BeginOutputReadLine();
            ////process.BeginErrorReadLine();
            //
            //// Synchronously read the standard output of the spawned process.
            //generatedOutput = process.StandardOutput.ReadToEnd();
            //
            //// Waits until program finishes before exiting
            //process.WaitForExit();
            StreamReader reader = process.StandardOutput;
            StringBuilder builder = new StringBuilder();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                builder.AppendLine(line);
            }

            string allLines = builder.ToString();
            generatedOutput = allLines;

            AssetDatabase.Refresh();
        }
        
    }

}
