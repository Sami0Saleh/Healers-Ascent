using UnityEngine;

public class ModelSliderController : MonoBehaviour
{
    private float minRotation = 85f;
    private float maxRotation = -85f;

    public void UpdateModel(float value)
    {
        float normalizedBrave = Mathf.InverseLerp(1, 100, value);
        float targetRotation = Mathf.Lerp(minRotation, maxRotation, normalizedBrave);

        // Set the rotation of the handle
        transform.localRotation = Quaternion.Euler(targetRotation, transform.localRotation.y, transform.localRotation.z);
    }
}
