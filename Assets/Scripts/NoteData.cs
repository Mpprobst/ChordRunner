using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NoteData : MonoBehaviour
{
    public int Note;
    private Material _material;

    void Start()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    public void SetDissolveAmount(float amount)
    {
        _material.SetFloat("_DissolveAmount", amount);
    }
}
