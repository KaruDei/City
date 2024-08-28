using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Place : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private PlaceData _placeData;
	public PlaceData PlaceData => _placeData;
	
	[Header("Map Component")]
	[SerializeField] private Map _map;
	
	[Header("Place Component")]
	[SerializeField] private Image _placeImage;
	
	[Header("Button Component")]
	[SerializeField] private Button _buttonComponent;
	[SerializeField] private Image _buttonImage;
	
	[Header("Name Field Component")]
	[SerializeField] private List<Image> _nameFieldImage;
	
	public bool IsAvailable;

	public void UpdatePlaceInfo()
	{
		if (IsAvailable)
		{
            if (_placeData.GetGamesLevelsCount() == _placeData.GetDoneGamesLevelsCount() && _placeData.GetGamesLevelsCount() == _placeData.GetFailGamesLevelsCount())
				SetPlaceInfo(_placeData.PlaceImageDone, _map.ButtonImageDone, _map.NameFieldImageDone);
			else
                SetPlaceInfo(_placeData.PlaceImageActive, _map.ButtonImageActive, _map.NameFieldImageDefault);
        }
		else
            SetPlaceInfo(_placeData.PlaceImageDisabled, _map.ButtonImageDisabled, _map.NameFieldImageDefault, false);
    }

	private void SetPlaceInfo(Sprite placeSprite, Sprite buttonSprite, Sprite nameFieldSprite, bool interactable = true)
	{
		_buttonComponent.interactable = interactable;
		_placeImage.sprite = placeSprite;
		_buttonImage.sprite = buttonSprite;
		foreach (Image name in _nameFieldImage)
            name.sprite = nameFieldSprite;
	}
}
