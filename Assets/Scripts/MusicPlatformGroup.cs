using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlatformGroup : MonoBehaviour
{
    public static MusicPlatformGroup Instance { get { return _instance; } }
    public static MusicPlatformGroup _instance;
    // 49 = C4 = 6 so, we subrtact 43 from midi value
    public Transform[] rows;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        Debug.Log($"C4 = {GetRowHeight("C4")}");
    }

    /// <summary>
    /// note must be in format <Note letter><octave>
    /// </summary>
    /// <param name="note"></param>
    /// <returns></returns>
    public float GetRowHeight(string note)
    {
        /*int n = note[0] - 'A';
        int octave = note[1] - '0';
        Debug.Log($"note: {note} to int -> {n} {octave}");
        int baseNote = rows[0].name[0];
        int baseOctave = rows[0].name[1];
        */
        return transform.Find(note).position.y;
    }

    public float GetRowHeight(int row)
    {
        row = Mathf.Clamp(row, 0, rows.Length-1);
        return rows[row].position.y;
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
}
