using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordManager : MonoBehaviour
{
    // this class takes an input of the notes and creates the chord objects for the player to navigate

    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private GameObject _chordPrefab;
    [SerializeField] private GameObject _notePrefab;
    [SerializeField] private float _beatDistance = 100f;
    private float _currentBeat;
    private MusicPlatformGroup _musicPlatformGroup;
    public float noteMovementSpeed = 1;

    void Update()
    {
        // TODO: Move bars across at set tempo, might be for a different class
    }

    private void FixedUpdate()
    {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x - noteMovementSpeed * Time.deltaTime, gameObject.transform.position.y);
    }

    /// <summary>
    /// Takes in values to generate the chords of the song
    /// </summary>
    /// <param name="chordList">The data received from Michael</param>
    public void CreateSong(List<MusicManager.BeatData> chordList)
    {
        _currentBeat = 0;
        List<GameObject> chordObjects = new List<GameObject>();
        // create a chord for each value in the list
        foreach (MusicManager.BeatData chordData in chordList)
        {
            _currentBeat++;
            if (!chordData.played)
                continue;

            int rootIndex = 0;

            _musicPlatformGroup = MusicPlatformGroup.Instance;
            foreach (NotePlayer notePlayer in _musicPlatformGroup.rows)
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


            GameObject chordObject = CreateChord(chordData.chordNotes, rootIndex, chordData.chord);

            chordObject.transform.localPosition += Vector3.right * _beatDistance * _currentBeat;

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
    public GameObject CreateChord(List<int> chordData, int root, string chordName)
    {
        GameObject chordObject = Instantiate(_chordPrefab, transform);

        ChordCollision chord = chordObject.GetComponent<ChordCollision>();
        chord.NoteDatas = new List<NoteData>();

        chord.Root = root;
        chord.ChordName = chordName;

        for(int i=0; i<chordData.Count; i++)
        {
            Vector2 notePosition = Vector2.up * _musicPlatformGroup.GetRowHeight(chordData[i]);
            NoteData note = Instantiate(_notePrefab, notePosition, Quaternion.identity, chordObject.transform).GetComponent<NoteData>();
            chord.NoteDatas.Add(note);
            note.Note = chordData[i];
        }

        return chordObject;
    }
}
