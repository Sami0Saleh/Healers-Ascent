using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image fillImage; 

    private Color redColor = new Color(1, 0, 0, 0.3f);
    private Color yellowColor = new Color(1, 1, 0, 0.3f);
    private Color greenColor = new Color(0, 1, 0, 0.3f);

    private void Start()
    {
        slider.minValue = 1;
        slider.maxValue = 100;
        
        UpdateSliderColor();
    }

    public void UpdateValue(int value)
    {
        value = Mathf.Clamp(value, 1, 100);
        slider.value = value;

        UpdateSliderColor();
    }

    private void UpdateSliderColor()
    {
        if (slider.value <= 40)
        {
            fillImage.color = redColor;
        }
        else if (slider.value >= 41 && slider.value <= 80)
        {
            fillImage.color = yellowColor;
        }
        else if (slider.value >= 81)
        {
            fillImage.color = greenColor;
        }
    }
}
