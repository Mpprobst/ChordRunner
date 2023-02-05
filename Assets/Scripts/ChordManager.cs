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
    [SerializeField] private GameObject _barPrefab;
    [SerializeField] private float _beatDistance = 0.25f;
    [SerializeField] private float _repeats = 4f;
    private List<ChordCollision> chordObjects;
    private float _currentBeat;
    private MusicPlatformGroup _musicPlatformGroup;
    private float noteMovementSpeed = 1;
    private float noteLength;
    private float lastNoteTime;
    float x_start, x_goal;

    private void Start()
    {
        // 32nd note length = 7.5f/bpm/2f
        noteLength =  15f / _musicManager.bpm;  
        noteMovementSpeed =  (1.66f) *_beatDistance / noteLength;
        lastNoteTime = -1000;
        noteMovementSpeed = _beatDistance * 8 * _musicManager.bpm / 60f;
        
        // note should travel its full distance in 32nd note
    }

    private void Update()
    {
        transform.localPosition -= new Vector3(Time.deltaTime * noteMovementSpeed, 0);
        foreach(ChordCollision chord in chordObjects)
        {
            chord.SetDissolveValue(Mathf.Clamp(-(chord.transform.position.x / 6), 0, 1));
        }
    }

    private void FixedUpdate()
    {
        //gameObject.transform.position = new Vector2(gameObject.transform.position.x - noteMovementSpeed * Time.deltaTime, gameObject.transform.position.y);
        /*if (Time.time - lastNoteTime > noteLength)
        {
            lastNoteTime = Time.time;
            x_start = transform.position.x;
            x_goal = x_start - _beatDistance;
        }
        float x = Mathf.Lerp(x_start, x_goal, (Time.time - lastNoteTime) / noteLength);
        transform.position = new Vector3(x, transform.position.y);*/
    }

    /// <summary>
    /// Takes in values to generate the chords of the song
    /// </summary>
    /// <param name="chordList">The data received from Michael</param>
    public void CreateSong(List<MusicManager.BeatData> chordList)
    {
        List<int> lastChord = new List<int>();
        chordObjects = new List<ChordCollision>();

        for (int r = 0; r < _repeats; r++)
        {
            // create a chord for each value in the list
            for (int i = 0; i < chordList.Count; i++)
            {
                if (_currentBeat % 16 == 1)
                    Instantiate(_barPrefab, new Vector3(_beatDistance * _currentBeat, 0, -2), Quaternion.identity, this.transform);

                _currentBeat += 1;
                MusicManager.BeatData chordData = chordList[i];

                Debug.Log(chordData.played);

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

                ChordCollision chordObject = CreateChord(chordData.chordNotes, chordData.otherNotes, rootIndex, chordData.chord, ((i + 1) % 2 == 0 || i == 0));
                chordObject.transform.localPosition = new Vector3(_beatDistance * _currentBeat, chordObject.transform.position.y, -2);
                chordObjects.Add(chordObject);
            }
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
    public ChordCollision CreateChord(List<int> chordData, List<int> otherNotes, int root, string chordName, bool visible)
    {
        GameObject chordObject = Instantiate(_chordPrefab, transform);

        ChordCollision chord = chordObject.GetComponent<ChordCollision>();
        chord.NoteDatas = new List<NoteData>();

        chord.Root = root;
        chord.ChordName = chordName;

        for (int i = 0; i < chordData.Count; i++)
        {
            Vector2 notePosition = new Vector2(0, _musicPlatformGroup.GetRowHeight(chordData[i]));
            NoteData note = Instantiate(_notePrefab, notePosition, Quaternion.identity, chordObject.transform).GetComponent<NoteData>();
            note.transform.localPosition = new Vector2(0, note.transform.localPosition.y);
            chord.NoteDatas.Add(note);
            note.Note = chordData[i];
        }

        for (int i = 0; i < otherNotes.Count; i++)
        {
            Vector2 notePosition = new Vector2(0, _musicPlatformGroup.GetRowHeight(otherNotes[i]));
            NoteData note = Instantiate(_invisibleNote, notePosition, Quaternion.identity, chordObject.transform).GetComponent<NoteData>();
            note.transform.localPosition = new Vector2(0, note.transform.localPosition.y); 
            chord.NoteDatas.Add(note);
            note.Note = otherNotes[i];
        }


        return chord;
    }
}
