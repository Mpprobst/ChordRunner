using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ChordCollision : MonoBehaviour
{
    public List<NoteData> NoteDatas;
    public int Root;
    private MusicPlatformGroup _musicPlatformGroup;

    // Start is called before the first frame update
    void Start()
    {
        NoteDatas = new List<NoteData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_musicPlatformGroup == null)
            _musicPlatformGroup = MusicPlatformGroup.Instance;

        /*foreach(value in _musicPlatformGroup.noteDataThing)
        {
            value.playNote();
        }*/
    }

    /// <summary>
    /// Sets the dissolve shader in the notes by the amount, except the root
    /// </summary>
    /// <param name="dissolveAmount">The amount of dissolving</param>
    public void SetDissolveValue(float dissolveAmount)
    {
        for(int i=0; i<NoteDatas.Count; i++)
            if(i != Root)
                NoteDatas[i].SetDissolveAmount(dissolveAmount);
    }
}
