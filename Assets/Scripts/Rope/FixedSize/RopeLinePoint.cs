using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLinePoint : MonoBehaviour
{
    float height;
    public Vector2 worldPosition { get; private set; }
    public Vector2 segmentEndPoint { get; private set; }
    private Vector2 direction;
    void Start()
    {
        height = GetComponent<CapsuleCollider2D>().bounds.extents.y * 2f;
    }

    public void UpdatePoints()
    {
        direction = transform.up;
        worldPosition = transform.position;

        float endX = worldPosition.x - (direction.x * height);
        float endY = worldPosition.y - (direction.y * height);
        segmentEndPoint = new Vector2(endX, endY);
    }
}
