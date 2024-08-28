using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableButton : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Quiz _quiz;
    [SerializeField] private Transform originalParent;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;

    private Vector3 _tempAnchoredPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_quiz.IsCorrectOrder)
            return;

        canvasGroup.alpha = 0.6f; // Делаем кнопку полупрозрачной
        canvasGroup.blocksRaycasts = false; // Отключаем блокировку лучей
        _tempAnchoredPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_quiz.IsCorrectOrder)
            return;
        rectTransform.anchoredPosition += eventData.delta / CanvasRef.GetScale(); // Перемещаем кнопку
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_quiz.IsCorrectOrder)
            return;
        canvasGroup.alpha = 1f; // Восстанавливаем непрозрачность
        canvasGroup.blocksRaycasts = true; // Включаем блокировку лучей

        // Проверяем, попала ли кнопка в другую кнопку
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.TryGetComponent<Button>(out var btn))
        {
            if (btn == null)
                return;

            Transform targetButton = btn.transform;
            if (targetButton != null && targetButton != originalParent)
            {
                // Меняем местами кнопки
                Transform parent = originalParent;

                // Получаем индексы
                int thisIndex = transform.GetSiblingIndex();
                int targetIndex = targetButton.GetSiblingIndex();

                // Меняем местами
                transform.SetParent(targetButton.parent);
                targetButton.SetParent(parent);

                // Устанавливаем индексы
                transform.SetSiblingIndex(targetIndex);
                targetButton.SetSiblingIndex(thisIndex);
            }
        }
        else
        {
            // Возвращаем кнопку на оригинальное место, если не попала в другую кнопку
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = _tempAnchoredPosition;
        }
    }
}