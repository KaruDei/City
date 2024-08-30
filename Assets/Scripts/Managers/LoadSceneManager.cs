using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _loadText;
	[SerializeField] private RectTransform _loadProgressBar;
	private static LoadSceneManager _instance;
	private AsyncOperation _loadingSceneOperation;

    private void Start()
    {
        _instance = this;
		SwitchToScene(1);
    }

    private void Update()
    {
        if (_loadingSceneOperation != null)
		{
            if (_loadingSceneOperation.progress >= 0.85f)
            {
                _loadText.text = $"Загрузка {Mathf.RoundToInt(1f * 100)}%";
                _loadProgressBar.localScale = new Vector2(Mathf.Lerp(_loadProgressBar.localScale.x, 1f, Time.deltaTime), _loadProgressBar.localScale.y);
            }
            else
            {
                _loadText.text = $"Загрузка {Mathf.RoundToInt(_loadingSceneOperation.progress * 100)}%";
                _loadProgressBar.localScale = new Vector2(Mathf.Lerp(_loadProgressBar.localScale.x, _loadingSceneOperation.progress, Time.deltaTime), _loadProgressBar.localScale.y);
            }
        }

		Invoke("DoneLoad", 3f);
    }

    public static void SwitchToScene(int index)
	{
		if (index < SceneManager.sceneCountInBuildSettings)
		{
			_instance._loadingSceneOperation = SceneManager.LoadSceneAsync(index);
			_instance._loadingSceneOperation.allowSceneActivation = false;
		}	
	}

	private void DoneLoad()
	{
        _instance._loadingSceneOperation.allowSceneActivation = true;
    }
}
