using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour
{
    //Zoom in by Scrolling
    [SerializeField] RectTransform puzzleContainer;
    [SerializeField] float zoomSpeed = 0.3f;
    [SerializeField] float minScale = 0.5f;
    [SerializeField] float maxScale = 3;
    //[SerializeField] float panSpeed = 1f;
    Vector2 initPosition = Vector2.zero;

    //Vector3 lastMousePosition;
    //bool isPanning = false;

    // Update is called once per frame
    void Update()
    {
        HandleZoom();
        //HandlePan();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float scaleChange = scroll * zoomSpeed;
            Vector3 newScale = puzzleContainer.localScale + new Vector3(scaleChange, scaleChange, scaleChange);
            newScale = new Vector3(
                Mathf.Clamp(newScale.x, minScale, maxScale),
                Mathf.Clamp(newScale.x, minScale, maxScale),
                1
            );
            puzzleContainer.localScale = newScale;
        }
    }

    /*void HandlePan()
    {
        if(Input.GetMouseButtonDown(1))
        {
            isPanning = true;
            lastMousePosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(1) && isPanning)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            puzzleContainer.anchoredPosition += new Vector2(delta.x, delta.y) * panSpeed;
            lastMousePosition = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(1))
        {
            isPanning = false;
        }
    }*/

    public void ResetVoidZoom()
    {
        puzzleContainer.localScale = new Vector3(1, 1, 1);
        //puzzleContainer.anchoredPosition = initPosition;
    }

    // Auto Scaling?
}
