using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteData : MonoBehaviour
{
    public int Note;
    private Material _material;

    void Start()
    {
        if (GetComponent<SpriteRenderer>())
            _material = GetComponent<SpriteRenderer>().material;
    }

    public void SetDissolveAmount(float amount)
    {
        if (_material)
            _material.SetFloat("_DissolveAmount", amount);
    }
}
