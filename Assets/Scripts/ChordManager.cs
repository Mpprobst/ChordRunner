using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordManager : MonoBehaviour
{
    // this class takes an input of the notes and creates the chord objects for the player to navigate

    [SerializeField] private GameObject _chordPrefab;
    [SerializeField] private GameObject _notePrefab;
    private MusicPlatformGroup _musicPlatformGroup;

    void Start()
    {
        _musicPlatformGroup = MusicPlatformGroup.Instance;
    }

    void Update()
    {
        // TODO: Move bars across at set tempo, might be for a different class
    }

    /// <summary>
    /// Takes in values to generate the chords of the song
    /// </summary>
    /// <param name="chordList">The data received from Michael</param>
    public void CreateSong(List<List<int>> chordList)
    {
        List<GameObject> chordObjects = new List<GameObject>();
        // create a chord for each value in the list
        foreach (List<int> chordData in chordList)
        {
            chordObjects.Add(CreateChord(chordData, 0)); // input index of root note
            // TODO: Set position of chord based on where it will be in the song
        }
    }

    /// <summary>
    /// Creates a group of notes TODO: Have special functionality for the root note and offset notes that are next to eachother
    /// </summary>
    /// <param name="chordData">The tuple values for the chord</param>
    /// <param name="root">Which note is the root note</param>
    /// <returns>The parent gameobject that contains the notes</returns>
    public GameObject CreateChord(List<int> chordData, int root)
    {
        GameObject chordObject = Instantiate(_chordPrefab, transform);

        ChordCollision chord = chordObject.GetComponent<ChordCollision>();

        chord.Root = root;

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
