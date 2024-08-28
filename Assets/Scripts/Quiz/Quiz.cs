using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Place Info Components")]
    [SerializeField] private PlaceInfo _placeInfo;
    [SerializeField] private Animator _placeInfoAnimator;

    [Header("Quiz Components")]
    [SerializeField] private TextMeshProUGUI _question;
    [SerializeField] private Image _timerImage;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private int _totalTimerTime;
    [SerializeField] private Transform _buttonAnswersParent;
    [SerializeField] private List<AnswerButton> _buttonAnswers;
    [SerializeField] private List<Button> _funtionalButtons;
    [SerializeField] private Animator _questionFieldAnimator;

    [Header("Windows Components")]
    [SerializeField] private GameObject _quizGameWindow;
    [SerializeField] private GameObject _quizWinWindow;
    [SerializeField] private Button _quizWinWindowRedyButton;

    private Place _tempPlace;
    private QuizData _tempQuizData;
    private bool _win;

    private bool _timerActive = false;
    private float _elapsedTimerTime = 0;

    private List<string> _answerList = new();

    public bool IsCorrectOrder = false;

    public void SetupQuiz(Place place)
    {
        _win = false;
        IsCorrectOrder = false;
        _answerList.Clear();
        foreach (var answerButton in _buttonAnswers)
        {
            answerButton.IsSelected = false;
            answerButton.UpdateColor();
            answerButton.gameObject.SetActive(true);
        }

        _tempPlace = place;
        _tempQuizData = GetQuizData();
        _question.text = _tempQuizData.Question;

        foreach (string answer in _tempQuizData.AnsversList)
            _answerList.Add(answer);
        
        Shuffle(_answerList);

        for (int i = 0; i < _buttonAnswers.Count && _buttonAnswers.Count == _answerList.Count; i++)
        {
            _buttonAnswers[i].SetupAnswerButton(_answerList[i]);
        }

        _elapsedTimerTime = 0f;
        _timerImage.fillAmount = 1f;
        _timerText.text = (_totalTimerTime - _elapsedTimerTime).ToString("0");
        _timerActive = true;

        SetFunctionalButtons();
    }

    private void FixedUpdate()
    {
        if (_timerActive)
        {
            _elapsedTimerTime += Time.fixedDeltaTime;
            _timerText.text = Mathf.RoundToInt(_totalTimerTime - _elapsedTimerTime).ToString();
            _timerImage.fillAmount = Mathf.Lerp(_timerImage.fillAmount, (_totalTimerTime - _elapsedTimerTime) / _totalTimerTime, Time.fixedDeltaTime);
            if (_elapsedTimerTime >= _totalTimerTime)
            {
                _elapsedTimerTime = _totalTimerTime;
                EndTime();
            }
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        System.Random rand = new();
        int n = list.Count;
        while (n > 1)
        {
            int k = rand.Next(n--);
            T temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }

    private void SetFunctionalButtons()
    {
        if (_tempQuizData.CorrectOrderFuntion && !_tempQuizData.FiftyFiftyFuntion)
        {
            _funtionalButtons[0].interactable = true;
            _funtionalButtons[1].interactable = false;
        }
        else if (!_tempQuizData.CorrectOrderFuntion && _tempQuizData.FiftyFiftyFuntion)
        {
            _funtionalButtons[0].interactable = false;
            _funtionalButtons[1].interactable = true;
        }
        else
        {
            _funtionalButtons[0].interactable = false;
            _funtionalButtons[1].interactable = false;
        }
    }

    private QuizData GetQuizData()
    {
        List<QuizData> quizDatas = new();
        for (int i = 0; i < _tempPlace.PlaceData.QuizDataList.Count; i++)
        {
            QuizData tempData = null;
            for (int j = 0; j < _tempPlace.PlaceData.QuizDoneList.Count && j < _tempPlace.PlaceData.QuizFailList.Count; j++)
            {
                if (_tempPlace.PlaceData.QuizDataList[i] == _tempPlace.PlaceData.QuizDoneList[j] || _tempPlace.PlaceData.QuizDataList[i] == _tempPlace.PlaceData.QuizFailList[j])
                {
                    tempData = _tempPlace.PlaceData.QuizDataList[i];
                    break;
                }
            }

            if (tempData == null)
                quizDatas.Add(_tempPlace.PlaceData.QuizDataList[i]);
        }

        int index = 0;
        if (quizDatas.Count > 0)
            index = Random.Range(0, quizDatas.Count - 1);

        return quizDatas[index];
    }

    public void CheckAnswer()
    {
        _timerActive = false;

        if (_tempQuizData.CorrectOrderFuntion && !_tempQuizData.FiftyFiftyFuntion)
        {
            if (IsCorrectOrder)
                IsCorrectOrder = false;

            List<AnswerButton> tempAnswerButtons = new();
            foreach (Transform answerButton in _buttonAnswersParent)
                tempAnswerButtons.Add(answerButton.GetComponent<AnswerButton>());

            for(int i = 0; i < _tempQuizData.CorrectAnsverList.Count && i < tempAnswerButtons.Count; i++)
            {
                if (_tempQuizData.CorrectAnsverList[i].Replace("\u200b", "").Trim().ToLower() != tempAnswerButtons[i].GetAnswer().Replace("\u200b", "").Trim().ToLower())
                {
                    _win = false;
                    AddData(null, _tempQuizData);
                    _questionFieldAnimator.SetTrigger("Fail");
                    Invoke("EndQuiz", 1f);
                    return;
                }
            }

            _win = true;
            AddData(_tempQuizData, null);
            _questionFieldAnimator.SetTrigger("Correct");
        }
        else if (!_tempQuizData.CorrectOrderFuntion && _tempQuizData.FiftyFiftyFuntion)
        {
            foreach (var buttonAnswer in _buttonAnswers)
                if (buttonAnswer.IsSelected)
                    if (_tempQuizData.AnsversList[0].Replace("\u200b", "").Trim().ToLower() == buttonAnswer.GetAnswer().Replace("\u200b", "").Trim().ToLower())
                    {
                        _win = true;
                        AddData(_tempQuizData, null);
                        _questionFieldAnimator.SetTrigger("Correct");
                    }

            if (!_win)
            {
                AddData(null, _tempQuizData);
                _questionFieldAnimator.SetTrigger("Fail");
            }
        }
        else
        {
            _win = false;
            AddData(null, _tempQuizData);
            _questionFieldAnimator.SetTrigger("Fail");
        }

        Invoke("EndQuiz", 1f);
    }

    private void AddData(QuizData done, QuizData fail)
    {
        _tempPlace.PlaceData.QuizDoneList.Add(done);
        _tempPlace.PlaceData.QuizFailList.Add(fail);
    }

    private void EndTime()
    {
        _timerActive = false;
        _win = false;
        AddData(null, _tempQuizData);
        _questionFieldAnimator.SetTrigger("Fail");

        Invoke("EndQuiz", 1f);
    }

    private void EndQuiz()
    {
        _quizGameWindow.SetActive(false);

        if (_win)
        {
            _quizWinWindow.SetActive(true);
            if (_tempPlace.PlaceData.QuizDataList.Count == _tempPlace.PlaceData.QuizDoneList.Count && _tempPlace.PlaceData.QuizDataList.Count == _tempPlace.PlaceData.QuizFailList.Count)
                _quizWinWindowRedyButton.gameObject.SetActive(false);
            else
                _quizWinWindowRedyButton.gameObject.SetActive(true);
        }
        else
        {
            _placeInfo.SetupPlaceInfo(_tempPlace);
            _placeInfoAnimator.SetTrigger("Open");
        }

        _tempPlace = null;
        _tempQuizData = null;

        Map.Instance.UpdatePlacesInfo();
    }

    public void ApplyFiftyFifty()
    {
        int count = 2;
        foreach (var answerButton in _buttonAnswers)
        {
            if (answerButton.GetAnswer().Replace("\u200b", "").Trim().ToLower() != _tempQuizData.AnsversList[0].Replace("\u200b", "").Trim().ToLower() && count > 0)
            {
                answerButton.IsSelected = false;
                answerButton.UpdateColor();
                answerButton.gameObject.SetActive(false);
                count--;
            }
        }
    }

    public void ApplyCorrectOrder()
    {
        IsCorrectOrder = !IsCorrectOrder;

        foreach (var answerButton in _buttonAnswers)
        {
            answerButton.IsSelected = false;
            answerButton.UpdateColor();
            answerButton.gameObject.SetActive(true);
        }
    }
}
