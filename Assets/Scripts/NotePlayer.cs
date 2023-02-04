using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePlayer : MonoBehaviour
{
    public int midiVal;
    private AudioSource source;
    private float basePitch;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        basePitch = MusicManager.NoteToPitch(midiVal);
        source.pitch = basePitch;
    }

    public void PlayNote(bool isSharp)
    {
        if (isSharp)
        {
            source.pitch = MusicManager.NoteToPitch(midiVal+1);
        }
        source.Play();
        source.pitch = basePitch;
    }
}
