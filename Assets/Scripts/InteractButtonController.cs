using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractButtonController : MonoBehaviour
{
    public Sprite resumeImage;
    public Sprite pauseImage;

    private bool isPlaying = true;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void changeImage()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            button.GetComponent<Image>().sprite = pauseImage;
            Time.timeScale = 1;
        }
        else
        {
            button.GetComponent<Image>().sprite = resumeImage;
            Time.timeScale = 0;
        }
    }
}
