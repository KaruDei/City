using UnityEngine;

public class CanvasRef : MonoBehaviour
{
	private static Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    public static float GetScale()
	{
		return _canvas.scaleFactor;
	}
}
