using UnityEngine;

[ExecuteAlways]
public class CircleLayout : MonoBehaviour
{
    [Header("Circle Settings")]
    public float radius = 5f;
    [Range(0f, 360f)] public float startAngleOffset = 0f;

    [Header("Editor Options")]
    public bool autoUpdate = true;

    private void OnValidate()
    {
        if (autoUpdate)
        {
            ArrangeInCircle();
        }
    }

    public void ArrangeInCircle()
    {
        int count = transform.childCount;
        if (count == 0) return;

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngleOffset + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
            transform.GetChild(i).localPosition = pos;
        }
    }
}
