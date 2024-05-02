using UnityEngine;

public class LaserTest : MonoBehaviour
{
    public void Setup(Vector3 position, Vector2 size)
    {
        transform.localPosition = position;
        GetComponent<SpriteRenderer>().size = size;
    }
}
