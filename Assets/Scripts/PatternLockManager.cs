using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternLockManager : MonoBehaviour
{
    public List<Image> patternImages; // List of images representing the pattern points
    public LineRenderer lineRenderer; // Line renderer for drawing connections
    public float lineZOffset = 0.1f; // Offset to ensure the line is visible

    private List<Image> patternPoints = new List<Image>(); // List to store pattern points
    private bool isDrawing = false; // Flag to check if drawing is in progress
    private Image lastTouchedImage; // Reference to the last touched image

    private void Start()
    {
        lineRenderer.positionCount = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
        }

        if (isDrawing)
        {
            UpdateDrawing();
        }
    }

    private void StartDrawing()
    {
        patternPoints.Clear();
        lineRenderer.positionCount = 0;

        isDrawing = true;
    }

    private void StopDrawing()
    {
        isDrawing = false;

        CheckPattern();
    }

    private void UpdateDrawing()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Image hitImage = hit.collider.GetComponent<Image>();
            if (patternImages.Contains(hitImage))
            {
                // If the mouse is over a pattern image, fix the line's end point to that image's position
                DrawLineToPoint(hitImage.rectTransform.position);

                // Add the current image to patternPoints only if it's different from the last one
                if (lastTouchedImage == null || lastTouchedImage != hitImage)
                {
                    patternPoints.Add(hitImage);
                    lastTouchedImage = hitImage;
                }
            }
        }
    }

    private void DrawLineToPoint(Vector3 point)
    {
        lineRenderer.positionCount = patternPoints.Count;
        lineRenderer.SetPositions(patternPoints.ConvertAll(p => p.rectTransform.position).ToArray());
    }

    private void CheckPattern()
    {
        // Convert the pattern points to a string representation
        string patternCode = string.Join("", patternPoints.ConvertAll(p => p.name).ToArray());

        Debug.Log("PATTERN received : " + patternCode);
        // Check if the pattern code matches "1236"
        if (patternCode == "1236")
        {
            Debug.Log("YES! Pattern lock matches the code 1236");
        }
        else
        {
            Debug.Log("Pattern lock does not match the code");
        }

        lineRenderer.positionCount = 0;
        patternPoints.Clear();
    }
}
