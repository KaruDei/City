using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;

    private void Start()
    {
        _instance = this;
    }

    public static void SetupPlace(Place place)
    {

    }

    public static void UpdateMapInfo()
    {
        
    }

    public void QuitGame()
	{
		Application.Quit();
	}
}
