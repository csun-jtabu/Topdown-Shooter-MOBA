using UnityEngine;
using System.Diagnostics;

public class GenerateMap : MonoBehaviour
{
    
    void Awake()
    {
        RunPython(); 
    }

    void RunPython()
    {
        // Python script path
        string scriptPath = Application.dataPath + "/Scripts/LevelGen/LevelGeneration.py"; 

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
            process.Start(); // starts the command line process

            // starts outputting
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Waits until program finishes before exiting
            process.WaitForExit();
        }
        
    }

}
