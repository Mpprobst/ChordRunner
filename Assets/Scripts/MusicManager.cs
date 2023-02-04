using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the creation of the song. 
/// Feeds chord information to the chord manager and instructs the chord manager when to spawn a chord
/// Determines the frequency of the note to be played
/// </summary>
public class MusicManager : MonoBehaviour
{
    // TODO: only look ahead 1 measure and populate those notes. At end of song repeat, and consider doing a key change
    public static float MID_C_FREQ = 261.64f;


    TextAsset encodedSong;
    // Start is called before the first frame update
    void Start()
    {
        MusicPlatformGroup notes = MusicPlatformGroup.Instance;
        foreach (var note in notes.rows)
        {
            float freq = MID_C_FREQ * Mathf.Pow(2, note.noteOffset / 12f); // equation to determine frequency of note based on its distance from middle c
            float pitch = freq / MID_C_FREQ;
            note.Initialize(pitch);
        }
        encodedSong = Resources.Load<TextAsset>("/SongFiles/encoded_song_19");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
