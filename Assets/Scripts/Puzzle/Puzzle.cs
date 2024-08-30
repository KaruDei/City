using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    [Header("Place Info Components")]
    [SerializeField] private PlaceInfo _placeInfo;
    [SerializeField] private Animator _placeInfoAnimator;

    [Header("Puzzle Components")]
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _puzzleElementsContainer;
    [SerializeField] private GridLayoutGroup _puzzleElementsLayoutGroup;
    [SerializeField] private RectTransform _puzzleImageElementsContainer;
    [SerializeField] private GridLayoutGroup _puzzleImageElementsLayoutGroup;
    [SerializeField] private Animator _puzzleImageFieldAnimator;

    [Header("Windows Components")]
    [SerializeField] private GameObject _puzzleGameWindow;
    [SerializeField] private GameObject _puzzleWinWindow;
    [SerializeField] private Button _redyButton;

    private Place _tempPlace;
    private PuzzleData _tempPuzzleData;
    private bool _win;

    public void SetupPuzzle(Place place)
    {
        _tempPlace = place;
        _tempPuzzleData = GetPuzzleData();

        if (_tempPuzzleData == null)
            EndPuzzle();

        ClearContainer();
        SliceSprite(_tempPuzzleData.Sprite, _tempPuzzleData.CountSlicesX, _tempPuzzleData.CountSlicesY);
        _redyButton.interactable = true;
    }

    private PuzzleData GetPuzzleData()
    {
        PuzzleData tempData = null;
        for (int i = 0; i < _tempPlace.PlaceData.PuzzleDataList.Count && tempData == null; i++)
        {
            if (i < _tempPlace.PlaceData.PuzzleDoneList.Count && i < _tempPlace.PlaceData.PuzzleFailList.Count)
            {
                if (_tempPlace.PlaceData.PuzzleDataList[i] != _tempPlace.PlaceData.PuzzleDoneList[i] && _tempPlace.PlaceData.PuzzleDoneList[i] != null)
                    tempData = _tempPlace.PlaceData.PuzzleDataList[i];
                else if (_tempPlace.PlaceData.PuzzleDataList[i] != _tempPlace.PlaceData.PuzzleFailList[i] && _tempPlace.PlaceData.PuzzleFailList[i] != null)
                    tempData = _tempPlace.PlaceData.PuzzleDataList[i];
            }
            else
                tempData = _tempPlace.PlaceData.PuzzleDataList[i];
        }

        return tempData;
    }

    public void CheckAnswer()
    {
        _redyButton.interactable = false;

        foreach (Transform elem in _puzzleImageElementsContainer)
        {
            if (elem.GetComponent<PuzzleElement>().IsDone == false)
            {
                _win = false;
                break;
            }
            else
                _win = true;
        }

        if (_win)
        {
            AddData(_tempPuzzleData, null);
            _puzzleImageFieldAnimator.SetTrigger("Correct");
        }
        else
        {
            AddData(null, _tempPuzzleData);
            _puzzleImageFieldAnimator.SetTrigger("Fail");
        }

        Invoke("EndPuzzle", 0.5f);
    }

    private void AddData(PuzzleData done, PuzzleData fail)
    {
        _tempPlace.PlaceData.PuzzleDoneList.Add(done);
        _tempPlace.PlaceData.PuzzleFailList.Add(fail);
    }

    private void EndPuzzle()
    {
        _puzzleGameWindow.SetActive(false);

        if (_win)
            _puzzleWinWindow.SetActive(true);
        else
        {
            _placeInfo.SetupPlaceInfo(_tempPlace);
            _placeInfoAnimator.SetTrigger("Open");
        }

        _tempPlace = null;
        _tempPuzzleData = null;
        _win = false;

        Map.Instance.UpdatePlacesInfo();
    }

    private void SliceSprite(Sprite sprite, int xCount, int yCount)
    {
        // Проверка на доступность текстуры
        if (!sprite.texture.isReadable)
        {
            Debug.LogError("Texture is not readable. Please check the import settings.");
            return;
        }

        Texture2D texture = sprite.texture;
        Rect spriteRect = sprite.rect;

        float pieceWidth = spriteRect.width / xCount;
        float pieceHeight = spriteRect.height / yCount;

        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
            {
                // Координаты для получения кусочка текстуры
                Rect pieceRect = new Rect(spriteRect.x + x * pieceWidth, spriteRect.y + y * pieceHeight, pieceWidth, pieceHeight);
                Texture2D pieceTexture = new Texture2D((int)pieceWidth, (int)pieceHeight);

                // Получаем пиксели из текстуры
                pieceTexture.SetPixels(texture.GetPixels((int)pieceRect.x, (int)pieceRect.y, (int)pieceWidth, (int)pieceHeight));
                pieceTexture.Apply();

                // Создаем новый спрайт из кусочка текстуры
                Sprite pieceSprite = Sprite.Create(pieceTexture, new Rect(0, 0, pieceTexture.width, pieceTexture.height), new Vector2(0.5f, 0.5f));

                // Инстанцируем префаб и назначаем ему спрайт
                GameObject puzzlePiece = Instantiate(_prefab, _puzzleElementsContainer);
                GameObject puzzleImagePiece = Instantiate(_prefab, _puzzleImageElementsContainer);
                puzzlePiece.GetComponent<Image>().sprite = pieceSprite;
                puzzlePiece.GetComponent<PuzzleElement>().Index = puzzlePiece.transform.GetSiblingIndex();

                puzzleImagePiece.GetComponent<Image>().sprite = pieceSprite;
                puzzleImagePiece.GetComponent<CanvasGroup>().alpha = 0.2f;
                puzzleImagePiece.GetComponent<PuzzleElement>().Index = puzzlePiece.transform.GetSiblingIndex();
                puzzleImagePiece.GetComponent<PuzzleElement>().CanMove = false;

                Vector3 currentRotation = transform.eulerAngles;
                currentRotation.z += RandomAngle();
                puzzlePiece.transform.eulerAngles = currentRotation;

                float SizeX = _puzzleImageElementsContainer.sizeDelta.x / xCount;
                float SizeY = _puzzleImageElementsContainer.sizeDelta.y / yCount;

                if (SizeX > 60f && SizeY > 60f)
                    _puzzleElementsLayoutGroup.constraintCount = 1;

                _puzzleElementsLayoutGroup.cellSize = new Vector2(SizeX, SizeY);
                _puzzleImageElementsLayoutGroup.cellSize = new Vector2(SizeX, SizeY);
            }
        }
    }

    private int RandomAngle()
    {
        List<int> angles = new() {0, 90, -180, -90};
        return angles[Random.Range(0, angles.Count - 1)];
    }

    private void ClearContainer()
    {
        foreach (Transform el in _puzzleElementsContainer)
            Destroy(el.gameObject);
        foreach (Transform el in _puzzleImageElementsContainer)
            Destroy(el.gameObject);
    }
}
