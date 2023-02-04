using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePlayer : MonoBehaviour
{
    public int midiVal;
    public int noteOffset; // distance from middle c
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Initialize(float pitch)
    {
        source = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public void PlayNote(bool isSharp)
    {

        source.Play();
    }
}
