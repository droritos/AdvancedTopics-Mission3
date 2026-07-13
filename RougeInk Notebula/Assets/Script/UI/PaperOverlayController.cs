using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PaperOverlayController : MonoBehaviour
{
    private Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        if (JuiceManager.Instance != null)
        {
            imageComponent.enabled = JuiceManager.Instance.enablePaperOverlay;
            JuiceManager.Instance.OnPaperOverlayToggled += ToggleOverlay;
        }
    }

    private void OnDestroy()
    {
        if (JuiceManager.Instance != null)
        {
            JuiceManager.Instance.OnPaperOverlayToggled -= ToggleOverlay;
        }
    }

    private void ToggleOverlay(bool isEnabled)
    {
        if (imageComponent != null)
        {
            imageComponent.enabled = isEnabled;
        }
    }
}
