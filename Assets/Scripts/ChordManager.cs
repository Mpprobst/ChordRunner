using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordManager : MonoBehaviour
{
    // this class takes an input of the notes and creates the chord objects for the player to navigate

    // subtract 43 to get the note index in the list of notes
    public static int NOTE_CONVERSION = 43;

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
    public void CreateSong(List<Tuple<int, int, int>> chordList)
    {
        List<GameObject> chordObjects = new List<GameObject>();
        // create a chord for each value in the list
        foreach (Tuple<int, int, int> chordData in chordList)
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
    public GameObject CreateChord(Tuple<int, int, int> chordData, int root)
    {
        GameObject chordObject = Instantiate(_chordPrefab, transform);
        _musicPlatformGroup.GetRowHeight(chordData.Item1);

        Vector2 note1Position = Vector2.up * _musicPlatformGroup.GetRowHeight(chordData.Item1 - NOTE_CONVERSION);
        Vector2 note2Position = Vector2.up * _musicPlatformGroup.GetRowHeight(chordData.Item2 - NOTE_CONVERSION);
        Vector2 note3Position = Vector2.up * _musicPlatformGroup.GetRowHeight(chordData.Item3 - NOTE_CONVERSION);

        Instantiate(_notePrefab, note1Position, Quaternion.identity, chordObject.transform);
        Instantiate(_notePrefab, note2Position, Quaternion.identity, chordObject.transform);
        Instantiate(_notePrefab, note3Position, Quaternion.identity, chordObject.transform);

        return chordObject;
    }
}
