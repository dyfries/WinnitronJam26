using UnityEngine;

// Helper class thats meant to be called from an Event. 
public class Twiddle_ChangeColor : MonoBehaviour
{
    public SpriteRenderer renderer;
    public Color targetColor;

    // Sets the Renderer to a given color. 
    public void ChangeColor() {
        renderer.color = targetColor;
    }

    // Sets the renderer to a random Hue (max Value, Saturation and Alpha Values. 
    public void RandomHue() {
        renderer.color = Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1); // Random Hue, fixed Saturation, Value and Alpha to max. 
    }
}
