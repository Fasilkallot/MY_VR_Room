using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Play a single video or play from a list of videos 
/// </summary>
[RequireComponent(typeof(VideoPlayer))]
public class PlayVideo : MonoBehaviour
{
    [Tooltip("Whether video should play on load")]
    public bool playAtStart = false;

    [Tooltip("Material used for playing the video (Uses URP/Unlit by default)")]
    public Material videoMaterial = null;

    [Tooltip("List of video clips to pull from")]
    public List<VideoClip> videoClips = new List<VideoClip>();

    [Tooltip("Time for to hold the power button to turn on and off the TV")]
    public float powerOnTime = 2.5f;

    private VideoPlayer videoPlayer = null;
    private MeshRenderer meshRenderer = null;

    private bool powerButton = false;
    private bool isTVOn = false;
    private bool isFolding = false;
    private float holdedTime = 0;

    private int index = 0;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoClips.Count > 0)
            videoPlayer.clip = videoClips[0];
    }

    private void OnEnable()
    {
        videoPlayer.prepareCompleted += ApplyVideoMaterial;
    }

    private void OnDisable()
    {
        videoPlayer.prepareCompleted -= ApplyVideoMaterial;
    }

    private void Start()
    {
        if (playAtStart)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }
    private void Update()
    {
        if (powerButton)
        {
            holdedTime += Time.deltaTime;

            if (holdedTime >= powerOnTime && !isFolding)
            {
                Stop();
                TVPowerControl();
            }

        }
        else if (isTVOn && holdedTime <= 1f && holdedTime > 0)
        {
            TogglePlayPause();
            holdedTime = 0;
        }
        else holdedTime = 0;
        
    }

    public void NextClip()
    {
        index = ++index % videoClips.Count;
        Play();
    }

    public void PreviousClip()
    {
        index = --index % videoClips.Count;
        Play();
    }

    public void RandomClip()
    {
        if (videoClips.Count > 0)
        {
            index = Random.Range(0, videoClips.Count);
            Play();
        }
    }

    public void PlayAtIndex(int value)
    {
        if (videoClips.Count > 0)
        {
            index = Mathf.Clamp(value, 0, videoClips.Count);
            Play();
        }
    }

    public void Play()
    {
        videoMaterial.color = Color.white;
        videoPlayer.Play();
    }

    public void Stop()
    {
        videoMaterial.color = Color.black;
        videoPlayer.Stop();
    }

    public void TogglePlayStop()
    {
        bool isPlaying = !videoPlayer.isPlaying;
        SetPlay(isPlaying);
    }

    public void TogglePlayPause()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
        else
            Play();
    }

    public void SetPlay(bool value)
    {
        if (value)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }

    private void ApplyVideoMaterial(VideoPlayer source)
    {
        meshRenderer.material = videoMaterial;
    }

    private void OnValidate()
    {
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        videoMaterial = mat;
    }

    public void TVPowerControl()
    {

        // Define the target Y position based on whether the TV is on or off
        float tvToPos = isTVOn ? 2.7f : 2.0f;

        StopCoroutine(MoveTV(tvToPos));

        StartCoroutine(MoveTV(tvToPos));

    }

    private IEnumerator MoveTV(float tvToPos)
    {
        isFolding = true;
        Transform parent = transform.parent;


        // Create the new position vector with the same X and Z coordinates but different Y coordinate
        Vector3 newPosition = new Vector3(parent.position.x, tvToPos, parent.position.z);


        while (Vector3.Distance(parent.position,newPosition) >= 0.01f)
        {
            // Move the parent GameObject towards the new position using Lerp
            parent.position = Vector3.Lerp(parent.position, newPosition, 2f * Time.deltaTime); 
            yield return null;
        }
        isFolding = false;
        isTVOn = !isTVOn;

    }

    public void PowerButtonPressed()
    {
        powerButton = true;
    }
    public void PowerButtonReleased()
    {
        powerButton = false;
    }
}