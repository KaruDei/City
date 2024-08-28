using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Places List")]
    [SerializeField] private List<Place> _places;

    [Header("Button Sprites")]
    public Sprite ButtonImageDone;
    public Sprite ButtonImageActive;
    public Sprite ButtonImageDisabled;

    [Header("Name Field Sprites")]
    public Sprite NameFieldImageDefault;
    public Sprite NameFieldImageDone;

    [Header("TopBar Components")]
    [SerializeField] private TextMeshProUGUI _puzzleGemsCountText;
    [SerializeField] private TextMeshProUGUI _rebusGemsCountText;
    [SerializeField] private TextMeshProUGUI _quizGemsCountText;
    [SerializeField] private RectTransform _progressBar;
    [SerializeField] private TextMeshProUGUI _progressBarProcentText;

    private float _progressTarget;

    public static Map Instance;

    private void Start()
    {
        Instance = this;
        UpdatePlacesInfo();
    }

    public void UpdatePlacesInfo()
    {
        foreach (Place place in _places)
            place.UpdatePlaceInfo();

        UpdateTopBarInfo();
    }

    private void UpdateTopBarInfo()
    {
        int puzzleGemsCount = 0, 
            rebusGemsCount = 0, 
            quizGemsCount = 0;
        
        foreach(Place place in _places)
        {
            foreach(PuzzleData puzzleData in place.PlaceData.PuzzleDoneList)
            {
                if (puzzleData != null)
                    puzzleGemsCount++;
            }
            foreach(RebusData rebusData in place.PlaceData.RebusDoneList)
            {
                if (rebusData != null)
                    rebusGemsCount++;
            }
            foreach(QuizData quizData in place.PlaceData.QuizDoneList)
            {
                if (quizData != null)
                    quizGemsCount++;
            }
        }

        _puzzleGemsCountText.text = $"{puzzleGemsCount}";
        _rebusGemsCountText.text = $"{rebusGemsCount}";
        _quizGemsCountText.text = $"{quizGemsCount}";

        UpdateProgressBar(puzzleGemsCount + rebusGemsCount + quizGemsCount);
    }

    private void UpdateProgressBar(float done)
    {
        float doneCount = done,
            allCount = 0;

        foreach (Place place in _places)
        {
            allCount += place.PlaceData.GetGamesLevelsCount();
        }

        _progressTarget = doneCount / allCount;
        _progressBarProcentText.text = (doneCount / allCount * 100).ToString("0.00") + "%";
    }

    private void FixedUpdate()
    {
        _progressBar.localScale = new Vector2(Mathf.Lerp(_progressBar.localScale.x, _progressTarget, Time.fixedDeltaTime), _progressBar.localScale.y);
    }
}
