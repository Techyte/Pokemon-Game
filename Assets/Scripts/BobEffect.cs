using UnityEngine;

public class BobEffect : MonoBehaviour
{
    public float bobIntensity;
    public float bobSpeed;
    float orignY;

    private void Start()
    {
        orignY = transform.position.y;
    }

    void Update()
    {
        Vector3 newPos = transform.position;
        newPos = transform.position;
        newPos.y = Mathf.Sin(Time.time * bobSpeed) * bobIntensity;
        newPos.y += orignY;

        transform.position = newPos;
    }
}
