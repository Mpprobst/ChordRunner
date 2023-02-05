using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteData : MonoBehaviour
{
    public int Note;
    private SpriteRenderer _renderer;

    void Start()
    {
        if (GetComponent<SpriteRenderer>())
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
    }

    public void SetDissolveAmount(float amount)
    {
        if (_renderer)
        {
            _renderer.material.SetFloat("_DissolveAmount", amount);
        }
    }
}
