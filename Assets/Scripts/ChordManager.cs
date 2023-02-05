using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordManager : MonoBehaviour
{
    // this class takes an input of the notes and creates the chord objects for the player to navigate

    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private GameObject _chordPrefab;
    [SerializeField] private GameObject _notePrefab, _invisibleNote;
    [SerializeField] private float _beatDistance = 0.25f;

    private float beatsCreated;
    private MusicPlatformGroup _musicPlatformGroup;
    private float noteMovementSpeed = 1;
    private float noteLength;
    float x_start, x_goal;


    private void Start()
    {
        noteLength =  15f / _musicManager.bpm;  
        noteMovementSpeed = _beatDistance / noteLength;
    }

    private void Update()
    {
        transform.position -= new Vector3(Time.deltaTime * noteMovementSpeed, 0);
    }

    private void FixedUpdate()
    {

    }

    /// <summary>
    /// Takes in values to generate the chords of the song
    /// </summary>
    /// <param name="chordList">The data received from Michael</param>
    public void CreateSong(List<MusicManager.BeatData> chordList)
    {
        List<int> lastChord = new List<int>();
        List<GameObject> chordObjects = new List<GameObject>();
        // create a chord for each value in the list
        for (int i = 0; i < chordList.Count; i++)
        {
            beatsCreated += 1;
            MusicManager.BeatData chordData = chordList[i];
            if (!chordData.played)
                continue;

            int rootIndex = 0;

            _musicPlatformGroup = MusicPlatformGroup.Instance;
            foreach (NotePlayer notePlayer in _musicPlatformGroup.rows)
            {
                if (notePlayer.gameObject.name[0] == char.ToUpper(chordData.chord[0]))
                {
                    int val = notePlayer.midiVal + (char.IsLower(chordData.chord[0]) ? 1 : 0);
                    //normalizing midi value to fall within a one octave range
                    while (val < 50)
                    {
                        val += 12;
                    }
                    while (val > 67)
                    {
                        val -= 12;
                    }
                    rootIndex = chordData.chordNotes.IndexOf(val);
                    if (rootIndex == -1)
                        Debug.Log("root not found");
                }
            }

            bool newChord = false;

            if (lastChord.Count == 3 && chordData.chordNotes.Count == 3)
            {
                for (int c = 0; c < chordData.chordNotes.Count; c++)
                {
                    if (chordData.chordNotes[c] != lastChord[c])
                    {
                        newChord = true;
                        break;
                    }
                }
            }

            if (lastChord.Count < 3)
                newChord = true;
            if (newChord)
                lastChord = chordData.chordNotes;

            GameObject chordObject = chordObject = CreateChord(chordData.chordNotes, chordData.otherNotes, rootIndex, chordData.chord, ((i + 1) % 2 == 0 || i == 0));
            chordObject.transform.localPosition += Vector3.right * _beatDistance * beatsCreated;
            chordObjects.Add(chordObject);
        }
    }

    /// <summary>
    /// Creates a group of notes TODO: Have special functionality for the root note and offset notes that are next to eachother
    /// </summary>
    /// <param name="chordData">The values for the chord</param>
    /// <param name="chordData">The tuple values for the chord</param>
    /// <param name="root">Which note is the root note</param>
    /// <param name="chordName">Name of the chord</param>
    /// <returns>The parent gameobject that contains the notes</returns>
    public GameObject CreateChord(List<int> chordData, List<int> otherNotes, int root, string chordName, bool visible)
    {
        GameObject chordObject = Instantiate(_chordPrefab, transform);

        ChordCollision chord = chordObject.GetComponent<ChordCollision>();
        chord.NoteDatas = new List<NoteData>();

        chord.Root = root;
        chord.ChordName = chordName;

        if (visible)
        {
            for (int i = 0; i < chordData.Count; i++)
            {
                Vector2 notePosition = Vector2.up * _musicPlatformGroup.GetRowHeight(chordData[i]);
                NoteData note = Instantiate(_notePrefab, notePosition, Quaternion.identity, chordObject.transform).GetComponent<NoteData>();
                note.transform.localPosition = new Vector2(0, note.transform.localPosition.y);
                chord.NoteDatas.Add(note);
                note.Note = chordData[i];
            }
        }

        for (int i = 0; i < otherNotes.Count; i++)
        {
            Vector2 notePosition = Vector2.up * _musicPlatformGroup.GetRowHeight(otherNotes[i]);
            NoteData note = Instantiate(_invisibleNote, notePosition, Quaternion.identity, chordObject.transform).GetComponent<NoteData>();
            note.transform.localPosition = new Vector2(0, note.transform.localPosition.y);
            chord.NoteDatas.Add(note);
            note.Note = otherNotes[i];
        }


        return chordObject;
    }
}
