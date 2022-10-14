using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOnOff : MonoBehaviour
{
    
    public Image on;
    public Image off;
    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetInt("soundOnOff");
    }

    // Update is called once per frame
    void Update()
    {
        if(AudioListener.volume == 0)
        {
            off.enabled = true;
            on.enabled = false;
        }
        else if (AudioListener.volume == 1)
        {
            off.enabled = false;
            on.enabled = true;
        }
    }

    public void ChangeOnOff()
    {
        if(AudioListener.volume == 0)
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("soundOnOff", 1);
        }
        else if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("soundOnOff", 0);
        }
    }
}
