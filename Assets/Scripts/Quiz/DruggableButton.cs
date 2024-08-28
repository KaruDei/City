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

        canvasGroup.alpha = 0.6f; // ������ ������ ��������������
        canvasGroup.blocksRaycasts = false; // ��������� ���������� �����
        _tempAnchoredPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_quiz.IsCorrectOrder)
            return;
        rectTransform.anchoredPosition += eventData.delta / CanvasRef.GetScale(); // ���������� ������
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_quiz.IsCorrectOrder)
            return;
        canvasGroup.alpha = 1f; // ��������������� ��������������
        canvasGroup.blocksRaycasts = true; // �������� ���������� �����

        // ���������, ������ �� ������ � ������ ������
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.TryGetComponent<Button>(out var btn))
        {
            if (btn == null)
                return;

            Transform targetButton = btn.transform;
            if (targetButton != null && targetButton != originalParent)
            {
                // ������ ������� ������
                Transform parent = originalParent;

                // �������� �������
                int thisIndex = transform.GetSiblingIndex();
                int targetIndex = targetButton.GetSiblingIndex();

                // ������ �������
                transform.SetParent(targetButton.parent);
                targetButton.SetParent(parent);

                // ������������� �������
                transform.SetSiblingIndex(targetIndex);
                targetButton.SetSiblingIndex(thisIndex);
            }
        }
        else
        {
            // ���������� ������ �� ������������ �����, ���� �� ������ � ������ ������
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = _tempAnchoredPosition;
        }
    }
}