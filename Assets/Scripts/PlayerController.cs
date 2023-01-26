using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private string currentNote = "C4";
    // Start is called before the first frame update
    void Start()
    {
        currentNote = "C4";
        MoveHeight(0);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: clean up the code :)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // move up
            MoveHeight(1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveHeight(-1);
            // TODO: move smoothly to the next line
        }
    }

    private void MoveHeight(int direction)
    {
        int row = MusicPlatformGroup.Instance.GetRowIdx(currentNote) + direction;
        float height = MusicPlatformGroup.Instance.GetRowHeight(row);
        transform.position = new Vector3(0, height, 0);
        currentNote = MusicPlatformGroup.Instance.GetRowName(row);
    }
}
