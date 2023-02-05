using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ChordCollision : MonoBehaviour
{
    public List<NoteData> NoteDatas;
    public int Root;
    public string ChordName;
    private MusicPlatformGroup _musicPlatformGroup;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided with " + collision.gameObject.name);
        if (_musicPlatformGroup == null)
            _musicPlatformGroup = MusicPlatformGroup.Instance;

        foreach(NoteData noteData in NoteDatas)
            _musicPlatformGroup.PlayNote(noteData.Note);


        float rootPos = -10;
        float playerPos = collision.gameObject.transform.position.y;
        if (Root >= 0)
            rootPos = NoteDatas[Root].transform.position.y;
        else
            Debug.Log("Root is out of range");
        if (Mathf.Abs(rootPos - playerPos) < Mathf.Epsilon)
        {
            Debug.Log("hit player");
        }
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

    public int GetRootMidiValue()
    {
        return NoteDatas[Root].Note;
    }
}
