using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuScript : MonoBehaviour
{
    public AudioSource aaudio;
    // Start is called before the first frame update
    void Start()
    {
        aaudio = GetComponent<AudioSource>();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
            aaudio.Play();
        }
    }
}
