using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceInfo : MonoBehaviour
{
	[Header("Place Info Components")]
	[SerializeField] private TextMeshProUGUI _titleText;
	[SerializeField] private Image _image;
	[SerializeField] private TextMeshProUGUI _descriptionText;

	[Header("Buttons")]
	[SerializeField] private List<Button> _buttons;

	[Header("Buttons Atempts")]
	[SerializeField] private List<Image> _puzzleButtonAttempts;
	[SerializeField] private List<Image> _rebusButtonAttempts;
	[SerializeField] private List<Image> _quizButtonAttempts;

	[Header("Atemps")]
	[SerializeField] private Sprite _attemptSpriteDefault;
	[SerializeField] private Sprite _attemptSpriteDone;
	[SerializeField] private Sprite _attemptSpriteFail;

    [Header("Games")]
    [SerializeField] private Rebus _rebus;
    [SerializeField] private Puzzle _puzzle;
    [SerializeField] private Quiz _quiz;

	private Place _tempPlace;

	public void SetupPlaceInfo(Place place)
	{
		_tempPlace = place;
		_titleText.text = _tempPlace.PlaceData.PlaceName;
		_image.sprite = _tempPlace.PlaceData.PlaceImage;
		_descriptionText.text = _tempPlace.PlaceData.PlaceInfo;

        SetupButtonsInfo();
        SetupInteractableButtons();
    }

    public void StartPuzzleGame()
    {
        _puzzle.SetupPuzzle(_tempPlace);
    }

    public void StartRebusGame()
    {
        _rebus.SetupRebus(_tempPlace);
    }

    public void StartQuizGame()
    {
        _quiz.SetupQuiz(_tempPlace);
    }

    private void SetupInteractableButtons()
    {
        if (_tempPlace.PlaceData.PuzzleDataList.Count == _tempPlace.PlaceData.PuzzleDoneList.Count && _tempPlace.PlaceData.PuzzleDataList.Count == _tempPlace.PlaceData.PuzzleFailList.Count)
            _buttons[0].interactable = false;
        else
            _buttons[0].interactable = true;

        if (_tempPlace.PlaceData.RebusDataList.Count == _tempPlace.PlaceData.RebusDoneList.Count && _tempPlace.PlaceData.RebusDataList.Count == _tempPlace.PlaceData.RebusFailList.Count)
            _buttons[1].interactable = false;
        else
            _buttons[1].interactable = true;

        if (_tempPlace.PlaceData.QuizDataList.Count == _tempPlace.PlaceData.QuizDoneList.Count && _tempPlace.PlaceData.QuizDataList.Count == _tempPlace.PlaceData.QuizFailList.Count)
            _buttons[2].interactable = false;
        else
            _buttons[2].interactable = true;
    }

	private void SetupButtonsInfo()
	{
        for (int i = 0; i < _puzzleButtonAttempts.Count; i++)
		{
			if (i < _tempPlace.PlaceData.PuzzleDoneList.Count && i < _tempPlace.PlaceData.PuzzleFailList.Count)
			{
				if (_tempPlace.PlaceData.PuzzleDoneList[i] != null)
                    _puzzleButtonAttempts[i].sprite = _attemptSpriteDone;
                else if (_tempPlace.PlaceData.PuzzleFailList[i] != null)
                    _puzzleButtonAttempts[i].sprite = _attemptSpriteFail;
				else
                    _puzzleButtonAttempts[i].sprite = _attemptSpriteDefault;
            }
			else
				_puzzleButtonAttempts[i].sprite = _attemptSpriteDefault;
		}

        for (int i = 0; i < _rebusButtonAttempts.Count; i++)
        {
            if (i < _tempPlace.PlaceData.RebusDoneList.Count && i < _tempPlace.PlaceData.RebusFailList.Count)
            {
                if (_tempPlace.PlaceData.RebusDoneList[i] != null)
                    _rebusButtonAttempts[i].sprite = _attemptSpriteDone;
                else if (_tempPlace.PlaceData.RebusFailList[i] != null)
                    _rebusButtonAttempts[i].sprite = _attemptSpriteFail;
                else
                    _rebusButtonAttempts[i].sprite = _attemptSpriteDefault;
            }
            else
                _rebusButtonAttempts[i].sprite = _attemptSpriteDefault;
        }

        for (int i = 0; i < _quizButtonAttempts.Count; i++)
        {
            if (i < _tempPlace.PlaceData.QuizDoneList.Count && i < _tempPlace.PlaceData.QuizFailList.Count)
            {
                if (_tempPlace.PlaceData.QuizDoneList[i] != null)
                    _quizButtonAttempts[i].sprite = _attemptSpriteDone;
                else if (_tempPlace.PlaceData.QuizFailList[i] != null)
                    _quizButtonAttempts[i].sprite = _attemptSpriteFail;
                else
                    _quizButtonAttempts[i].sprite = _attemptSpriteDefault;
            }
            else
                _quizButtonAttempts[i].sprite = _attemptSpriteDefault;
        }
    }
}
