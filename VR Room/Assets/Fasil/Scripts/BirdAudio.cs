using System.Collections;
using UnityEngine;

public class BirdAudio : MonoBehaviour
{
    [SerializeField] AudioClip[] birdAudio;
    [SerializeField] float maxAudioDelay=10;

    AudioSource birdAudioSource;

    private void Start()
    {
        birdAudioSource = GetComponent<AudioSource>();

        StartCoroutine(PlayBirdSound());
    }

    IEnumerator PlayBirdSound()
    {
        while (true)
        {
            AudioClip clip = birdAudio[Random.Range(0,birdAudio.Length)];
            birdAudioSource.PlayOneShot(clip);

            yield return new WaitForSeconds(Random.Range(0,maxAudioDelay));
        }
    }
}
