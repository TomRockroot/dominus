using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using D_StructsAndEnums;

public class D_SoundMaster : MonoBehaviour
{
    public AudioSource mSoundSourceUI;

    public AudioClip mErrorSound;
    public List<AudioClip> mClickUI = new List<AudioClip>();
    public List<AudioClip> mWoodCut = new List<AudioClip>();

    private static D_SoundMaster SOUND_MASTER;

    public static D_SoundMaster GetInstance()
    {
        if(SOUND_MASTER == null)
        {
            SOUND_MASTER = FindObjectOfType<D_SoundMaster>();
        }
        return SOUND_MASTER;
    }

    public void PlayError()
    {
        mSoundSourceUI.PlayOneShot(mErrorSound);
    }

    public void PlaySound(ESoundCue cue, int variation = 0)
    {
        List<AudioClip> clips = new List<AudioClip>();
        AudioClip toPlay;

        switch(cue)
        {
            case ESoundCue.SC_ClickUI:
                clips = mClickUI;
                break;
            case ESoundCue.SC_WoodCut:
                clips = mWoodCut;
                break;
        }

        if(clips.Count < 1)
        {
            Debug.LogError("SOUND_MASTER: No SoundClips for " + cue);
            PlayError();
            return;
        }

        // play random sound, if variation negative (ideally -1)
        if (variation < 0)
        {
            int rnd = Random.Range(0, clips.Count - 1);
            toPlay = clips[rnd];

            if (toPlay != null)
            {
                mSoundSourceUI.PlayOneShot(toPlay);
                return;
            }
            Debug.LogError("SOUND_MASTER: Clip for " + cue + " at " + rnd + " was null!");
            PlayError();
            return;
        }

        if (variation < clips.Count)
        {
            toPlay = clips[variation];

            if (toPlay != null)
            {
                mSoundSourceUI.PlayOneShot(toPlay);
                return;
            }
            Debug.LogError("SOUND_MASTER: Clip for " + cue + " at " + variation + " was null!");
            PlayError();
            return;
        }

        Debug.LogWarning("SOUND_MASTER: variation " + variation + " was not available for " + cue + " as it has only " + clips.Count +" clips.");
        PlayError();
    }
}
