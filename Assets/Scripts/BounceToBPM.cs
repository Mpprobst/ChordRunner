using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BounceToBPM : MonoBehaviour
{
    [SerializeField] private MusicManager _musicManager;
    private SpriteRenderer _renderer;
    private float _bpm = 0;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _bpm = _musicManager.bpm;
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.sharedMaterial.SetFloat("_GradientAmount", 0.5f + (Mathf.Sin(Time.time * _musicManager.bpm / 30f) / 2));
    }
}
