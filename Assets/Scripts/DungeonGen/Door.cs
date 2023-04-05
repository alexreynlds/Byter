using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        top,
        bottom,
        left,
        right
    }

    public DoorType doorType;
}
