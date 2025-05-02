using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicLow;
    public AudioSource musicHigh;
    private bool musicToggled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ToggleMusic()
    {
        if (!musicToggled)
        {
            musicToggled = true;
            musicHigh.volume = 0.15f;
            musicLow.volume = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
