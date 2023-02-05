using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlatformGroup : MonoBehaviour
{
    public static MusicPlatformGroup Instance { get { return _instance; } }
    public static MusicPlatformGroup _instance;
    NotePlayer previousNote = null;

    public NotePlayer[] rows;
    public AudioSource[] extraSources;
    private bool[] sourcesUsed;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        sourcesUsed = new bool[extraSources.Length];
    }

    /// <summary>
    /// note must be in format <Note letter><octave>
    /// </summary>
    /// <param name="note"></param>
    /// <returns></returns>
    public float GetRowHeight(int midi)
    {
        foreach (var r in rows)
            if (r.midiVal == midi)
                return r.transform.position.y;
        foreach (var r in rows)
            if (r.midiVal == midi - 1)
                return r.transform.position.y;
        Debug.Log("Failed to find: " + midi);
        return 0f;
        /*int n = note[0] - 'A';
        int octave = note[1] - '0';
        Debug.Log($"note: {note} to int -> {n} {octave}");
        int baseNote = rows[0].name[0];
        int baseOctave = rows[0].name[1];
        */
    }

    public int GetRowIdx(string note)
    {
         return transform.Find(note).GetSiblingIndex();
    }

    public string GetRowName(int row)
    {
        row = Mathf.Clamp(row, 0, rows.Length - 1);
        return rows[row].name;
    }

    public void PlayNote(int midiValue)
    {
        for (int i=0; i<rows.Length; i++)
        {
            NotePlayer note = rows[i];
            if(note.midiVal == midiValue)
            {
                note.PlayNote(false);
                return;
            }

            if (previousNote)
            {
                if (note.midiVal > midiValue && previousNote.midiVal < midiValue)
                {

                    previousNote.PlayNote(true);
                    return;
                }
            }

            previousNote = note;
        }

        // note not found. improvise
        for (int i = 0; i < extraSources.Length; i++)
        {
            if (!sourcesUsed[i])
            {
                extraSources[i].pitch = MusicManager.NoteToPitch(midiValue);
                extraSources[i].Play();
                sourcesUsed[i] = true;
                Invoke("ClearExtraSources", extraSources[i].clip.length);
            }
        }
    }

    private void ClearExtraSources()
    {
        for (int i = 0; i < sourcesUsed.Length; i++)
            sourcesUsed[i] = false;
    }
}
