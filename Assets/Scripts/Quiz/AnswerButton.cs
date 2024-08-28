using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Quiz _quiz;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _answer;
    [SerializeField] private List<AnswerButton> _buttons;

	public bool IsSelected;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_quiz.IsCorrectOrder)
            return;

        IsSelected = !IsSelected;
        ResetButtons();
        UpdateColor();
    }

    public void SetupAnswerButton(string text)
    {
        _image.color = Color.white;
        _answer.text = text;
    }

    public void UpdateColor()
    {
        if (IsSelected)
            _image.color = Color.green;
        else
            _image.color = Color.white;
    }

    public string GetAnswer()
    {
        return _answer.text;
    }

    public void ResetButtons()
    {
        foreach (var button in _buttons)
        {
            if (button != this)
            {
                button.IsSelected = false;
                button.UpdateColor();
            }
        }
    }
}
