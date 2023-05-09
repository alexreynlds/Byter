using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public static MiniMapScript instance;

    public Room currentRoom;

    public float cameraMoveSpeed = 5f;

    public float cameraOffset = -0.5f;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (currentRoom == null) return;

        Vector3 targetPos = GetCameraTargetPosition();

        transform.position =
            Vector3
                .Lerp(transform.position,
                targetPos,
                cameraMoveSpeed * Time.deltaTime);
    }

    Vector3 GetCameraTargetPosition()
    {
        if (currentRoom == null) return Vector3.zero;

        Vector3 targetPos = currentRoom.GetRoomCenter();
        targetPos.x += cameraOffset;
        targetPos.y += cameraOffset;

        targetPos.z = transform.position.z;

        return targetPos;
    }

    public bool IsSwitchingRoom()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
