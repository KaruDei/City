using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleElement : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Vector3 _tempAnchoredPosition;
    private Transform _parent;
    private Transform _tempParent;

    public bool IsDone = false;
    public bool CanMove = true;
    public int Index;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanMove) return;
        else if (_parent == null || _tempParent == null)
        {
            _parent = transform.parent;
            _tempParent = transform.parent.parent.parent;
        }
        // Добавление условия и переменной на правильную постановку
        // Или можно удалять объект и делать прозрачность элемента в картинке в 1

        _canvasGroup.alpha = 0.6f; // Делаем кнопку полупрозрачной
        _canvasGroup.blocksRaycasts = false; // Отключаем блокировку лучей
        _tempAnchoredPosition = _rectTransform.anchoredPosition;
        transform.SetParent(_tempParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanMove) return;
        else if (_parent == null || _tempParent == null)
        {
            _parent = transform.parent;
            _tempParent = transform.parent.parent.parent;
        }
        _rectTransform.anchoredPosition += eventData.delta / CanvasRef.GetScale();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanMove) return;
        else if (_parent == null || _tempParent == null)
        {
            _parent = transform.parent;
            _tempParent = transform.parent.parent.parent;
        }

        _canvasGroup.alpha = 1f; // Восстанавливаем непрозрачность
        _canvasGroup.blocksRaycasts = true; // Включаем блокировку лучей

        // Проверяем, попала ли кнопка в другую кнопку
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.TryGetComponent<PuzzleElement>(out var puzzleElem))
        {
            if (puzzleElem == null)
                return;

            if (puzzleElem != null && puzzleElem != _parent && puzzleElem != _tempParent && puzzleElem.IsDone == false)
            {
                if (puzzleElem.Index == Index && Mathf.RoundToInt(transform.eulerAngles.z) == 0f)
                {
                    puzzleElem.IsDone = true;
                    puzzleElem.GetComponent<CanvasGroup>().alpha = 1f;
                    Destroy(gameObject);
                }
                else
                {
                    transform.SetParent(_parent);
                    _rectTransform.anchoredPosition = _tempAnchoredPosition;
                }
            }
            else
            {
                transform.SetParent(_parent);
                _rectTransform.anchoredPosition = _tempAnchoredPosition;
            }
        }
        else
        {
            // Возвращаем кнопку на оригинальное место, если не попала в другую кнопку
            transform.SetParent(_parent);
            _rectTransform.anchoredPosition = _tempAnchoredPosition;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CanMove) return;
        else if (_parent == null || _tempParent == null)
        {
            _parent = transform.parent;
            _tempParent = transform.parent.parent.parent;
        }
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z += 90f;
        transform.eulerAngles = currentRotation;
    }
}
