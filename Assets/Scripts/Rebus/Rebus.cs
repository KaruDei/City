using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rebus : MonoBehaviour
{
	[Header("Place Info Components")]
	[SerializeField] private PlaceInfo _placeInfo;
	[SerializeField] private Animator _placeInfoAnimator;

	[Header("Rebus Components")]
	[SerializeField] private Image _image;
	[SerializeField] private TextMeshProUGUI _inputText;
	[SerializeField] private Animator _inputAnimator;

	[Header("Windows Components")]
	[SerializeField] private GameObject _rebusGameWindow;
	[SerializeField] private GameObject _rebusWinWindow;
	[SerializeField] private Button _rebusWinWindowRedyButton;

    private Place _tempPlace;
    private RebusData _tempRebusData;
	private bool _win;

	public void SetupRebus(Place place)
	{
		_win = false;
		_inputText.text = "";
        _tempPlace = place;
		_tempRebusData = GetRebusData();

		_image.sprite = _tempRebusData.Sprite;
    }

	private RebusData GetRebusData()
	{
		List<RebusData> rebusDatas = new();
		for (int i = 0; i < _tempPlace.PlaceData.RebusDataList.Count; i++)
		{
			RebusData tempData = null;
			for (int j = 0; j < _tempPlace.PlaceData.RebusDoneList.Count && j < _tempPlace.PlaceData.RebusFailList.Count; j++)
			{
				if (_tempPlace.PlaceData.RebusDataList[i] == _tempPlace.PlaceData.RebusDoneList[j] || _tempPlace.PlaceData.RebusDataList[i] == _tempPlace.PlaceData.RebusFailList[j])
				{
					tempData = _tempPlace.PlaceData.RebusDataList[i];
					break;
                }
			}

			if (tempData == null)
				rebusDatas.Add(_tempPlace.PlaceData.RebusDataList[i]);
		}

		int index = 0;
        if (rebusDatas.Count > 0)
			index = Random.Range(0, rebusDatas.Count - 1);

		return rebusDatas[index];
	}

	public void CheckAnswer(TextMeshProUGUI input)
	{
		if (input.text.Replace("\u200b", "").Trim().ToLower() == _tempRebusData.Answer.ToLower())
		{
			_win = true;
			AddData(_tempRebusData, null);
            _inputAnimator.SetTrigger("Correct");
        }
		else
		{
            _win = false;
			AddData(null, _tempRebusData);
			_inputAnimator.SetTrigger("Fail");
        }

		Invoke("EndRebus", 1f);
	}

	private void AddData(RebusData done, RebusData fail)
	{
		_tempPlace.PlaceData.RebusDoneList.Add(done);
		_tempPlace.PlaceData.RebusFailList.Add(fail);
	}

	private void EndRebus()
	{
        _rebusGameWindow.SetActive(false);

        if (_win)
		{
			_rebusWinWindow.SetActive(true);
			if (_tempPlace.PlaceData.RebusDataList.Count == _tempPlace.PlaceData.RebusDoneList.Count && _tempPlace.PlaceData.RebusDataList.Count == _tempPlace.PlaceData.RebusFailList.Count)
				_rebusWinWindowRedyButton.gameObject.SetActive(false);
			else
                _rebusWinWindowRedyButton.gameObject.SetActive(true);
        }
		else
		{
			_placeInfo.SetupPlaceInfo(_tempPlace);
            _placeInfoAnimator.SetTrigger("Open");
		}

        _tempPlace = null;
        _tempRebusData = null;

        Map.Instance.UpdatePlacesInfo();
    }
}
