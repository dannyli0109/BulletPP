using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    RectTransform targetRect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(RectTransform rect, string content, string header = "")
    {
        targetRect = rect;
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.richText = true;
        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }


    private void LateUpdate()
    {

        //if (Application.isEditor)
        //{
        //    int headerLength = headerField.text.Length;
        //    int contentLength = contentField.text.Length;

        //    layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
        //}
        RectTransform rt = rectTransform;
        float width = rt.rect.width;
        float height = rt.rect.height;

        Vector2 position;
        if (targetRect.transform.position.x > Screen.width / 2.0f)
        {
            position = targetRect.transform.position + new Vector3(-targetRect.rect.width / 2.0f - width / 2.0f, 0, 0);
        }
        else
        {
            position = targetRect.transform.position + new Vector3(targetRect.rect.width / 2.0f + width / 2.0f, 0, 0);
        }

        //float pivotX = position.x / Screen.width;
        //float pivotY = position.y / Screen.height;

        //rectTransform.pivot = new Vector2(0, 0.5f);
        transform.position = position;
    }
}
