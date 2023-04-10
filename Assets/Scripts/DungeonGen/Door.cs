using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorDir
    {
        up,
        down,
        left,
        right
    }

    public DoorDir doorDir;
}
