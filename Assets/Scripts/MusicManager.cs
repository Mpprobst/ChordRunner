using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Manages the creation of the song. 
/// Feeds chord information to the chord manager and instructs the chord manager when to spawn a chord
/// Determines the frequency of the note to be played
/// </summary>
public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public struct DrumBeat
    {
        public List<bool> drumSounds;
    }
    [SerializeField] private ChordManager _chordManager;
    public string songName;
    public float bpm = 100;
    public AudioSource songSource, hiHat, snare, drumKick;
    [SerializeField] private DrumBeat[] drumPattern; // should be 16

    private float _beatInterval, _beatTimer;
    private int _beatCountFull, measureCount;
    private bool _beatFull;

    private bool doPulse;
    public float _pulseInterval, _pulseTimer, _pulseStart, _pulseEnd, _pulseDirection;
    public Material[] gradientMats;

    // bps = bpm/60f. 
    public struct BeatData
    {
        public bool played;
        public string chord;
        public List<int> chordNotes;
        public List<int> otherNotes;
    }

    // TODO: only look ahead 1 measure and populate those notes. At end of song repeat, and consider doing a key change
    public static float MID_C_FREQ = 261.64f;
    public static int MID_C_MIDI = 60;
    public List<BeatData> BeatDatas;

    TextAsset encodedSong;

    // Start is called before the first frame update
    void Start()
    {
        MusicPlatformGroup notes = MusicPlatformGroup.Instance;
        encodedSong = Resources.Load<TextAsset>($"SongFiles/{songName}");
        BeatDatas = ParseSong(encodedSong);

        if (_chordManager != null)
        {
            _chordManager.CreateSong(BeatDatas);
            _chordManager.CreateSong(BeatDatas);
        }

    }

    // Update is called once per frame
    void Update()
    {
        BeatDetection();
        Pulse();
    }

    private void Pulse()
    {
        _pulseTimer += Time.deltaTime * _pulseDirection;

        if (_pulseTimer >= _pulseInterval)
            _pulseDirection = -1;
        else if (_pulseTimer <= 0)
        {
            _pulseDirection = 1;
            if (_pulseEnd == 0)
                _pulseEnd = 0.6f;
        }


        float amount = Mathf.Lerp(_pulseStart, _pulseEnd, _pulseTimer / _pulseInterval);
        //Debug.Log($"Pulse: {_pulseTimer} dir: {_pulseDirection} interval: {_pulseInterval} = amount {amount}");

        foreach (var mat in gradientMats)
            mat.SetFloat("_GradientAmount", amount);

    }

    private void BeatDetection()
    {
        _beatFull = false;
        _beatInterval = 30f / bpm;
        _beatTimer += Time.deltaTime;
        if (_beatTimer >= _beatInterval)
        {
            if (_beatCountFull % 4 == 0)
            {
                _pulseInterval = _beatInterval / 4f;
                _pulseDirection = 1;
                _pulseTimer = 0;
                _pulseEnd = 0;
                doPulse = true;
                drumKick.Play();
            }
            else if (_beatCountFull % 4 == 1)
            {
                //_pulseInterval = _beatInterval / 8f;
                _pulseEnd = 0.6f;
            }

            /*if (drumPattern[_beatCountFull].drumSounds[0])
                drumKick.Play();
            if (drumPattern[_beatCountFull].drumSounds[1])
                hiHat.Play();
            if (drumPattern[_beatCountFull].drumSounds[2])
                snare.Play();
            */
            _beatTimer -= _beatInterval;
            _beatFull = true;
            _beatCountFull++;

            if (_beatCountFull > 15)
            {
                _beatCountFull = 0;
                measureCount++;
                if (measureCount % 4 == 0)
                    ParseSong(encodedSong);
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="noteOffset"> number of notes away from C4</param>
    /// <returns></returns>
    public static float NoteToPitch(int note)
    {
        float freq = MID_C_FREQ * Mathf.Pow(2, (MID_C_MIDI - note) / 12f); // equation to determine frequency of note based on its distance from middle c
        float pitch = freq / MID_C_FREQ;
        return pitch;
    }

    /// <summary>
    /// Takes path to fike containing song data that has 4 elements on a single line separated by white space:
    /// 1 or 0 whether or not the note was played this beat or not
    /// name of the chord, first letter indicating the chord name, uppercase for standard, lowercase indicating sharp
    ///     second characcter will be i or j indicating minor or major, respectively
    /// array of 3 notes comprising of the main chord in midi values
    /// list of other notes playing in this beat
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public List<BeatData> ParseSong(TextAsset file)
    {
        // Data is badly formatted. sorry :(
        List<BeatData> songData = new List<BeatData>();
        string fs = file.text;
        string[] fLines = fs.Split('\n');

        for (int l = 0; l < fLines.Length; l++)
        {
            string line = fLines[l];
            if (line.Length < 16) continue;
            BeatData beat = new BeatData();
            beat.played = line[0] - '0' == 1;

            beat.chord = line.Substring(2, 2);

            int arrayStart = 6;
            int arrayEnd = line.IndexOf(']', 0);

            string chordString = line.Substring(arrayStart, arrayEnd-arrayStart);
            string[] chordArray = chordString.Split(',');
            List<int> chord = new List<int>();
            if (arrayEnd - arrayStart >= 2)
            {
                for (int i = 0; i < chordArray.Length; i++)
                {
                    int val = int.Parse(chordArray[i].Trim());

                    //normalizing midi value to fall within a one octave range
                    while (val < 50)
                    {
                        val += 12;
                    }
                    while (val > 67)
                    {
                        val -= 12;
                    }
                    chord.Add(val);
                }
            }
            beat.chordNotes = chord;
            
            arrayStart = line.IndexOf('[', arrayEnd);
            arrayEnd = line.IndexOf(']', arrayStart);
            List<int> otherNotes = new List<int>();
            /*
            if (arrayEnd - arrayStart >= 2)
            {
                arrayStart++;   // so first char is not [
                chordString = line.Substring(arrayStart, arrayEnd - arrayStart);
                chordArray = chordString.Split(',');
                for (int i = 0; i < chordArray.Length; i++)
                {
                    int val = int.Parse(chordArray[i].Trim());
                    otherNotes.Add(val);
                }
            }
            */
            beat.otherNotes = otherNotes;
            songData.Add(beat);
        }
        return songData;
    }

    public void StartSong()
    {
        if (!songSource.isPlaying)
            songSource.Play();
    }
}
