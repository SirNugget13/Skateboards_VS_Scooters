using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioMixer GameAudio;
    
    public AudioSource mainMenu;
    public AudioSource dungeon1;
    public AudioSource dungeon2;
    public AudioSource merchant;
    public AudioSource death;

    public AudioSource trackPlaying;

    private void Start()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (activeSceneIndex == 0)
        {
            PlayTrack(mainMenu);
        }

        if(activeSceneIndex == 1)
        {
            int random = Random.Range(0, 2);

            if(random == 0)
            {
                PlayTrack(dungeon1);
            }
            else
            {
                PlayTrack(dungeon2);
            }
        }

        if(activeSceneIndex == 2)
        {
            PlayTrack(merchant);
        }
    }

    public void SwitchTrack(AudioSource trackToPlay)
    {
        
        StartCoroutine(FadeMixerGroup.StartFade(GameAudio, "MusicVolume", 1.5f, 0));

        this.Wait(1.5f, () =>
        {
            trackPlaying.Stop();
            PlayTrack(trackToPlay);

            //StartCoroutine(FadeMixerGroup.StartFade(GameAudio, "MusicVolume", 0.01f, 0));
            StartCoroutine(FadeMixerGroup.StartFade(GameAudio, "MusicVolume", 3f, 0.8f));
        });

    }

    public void TurnOffEffects()
    {
        StartCoroutine(FadeMixerGroup.StartFade(GameAudio, "EffectsVolume", 1f, 0f));
    }

    public void PlayTrack(AudioSource track)
    {
        track.Play();
        trackPlaying = track;
    }
}
