using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FPSCounter : MonoBehaviour
{
    [SerializeField] Text fpsText;

    // Frame rate calculation
    private int queueMaxCount = 20;
    private Queue<float> frameTimeQueue = new Queue<float>();
    private float avgFrameRate = 0f;
    private int frameCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update framerate in UI
        frameTimeQueue.Enqueue(Time.time);
        frameCounter++;
        if (frameTimeQueue.Count > queueMaxCount)
        { // overflow
            frameTimeQueue.Dequeue();
        }
        if (frameCounter >= queueMaxCount || frameTimeQueue.Count <= 5)
        { // update frame rate
            frameCounter = 0;
            avgFrameRate = frameTimeQueue.Count / (Time.time - frameTimeQueue.Peek());
            fpsText.text = avgFrameRate.ToString("F1") + " FPS";
        }
    }
}
