using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class RopeLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    private enum PlotMode { AVERAGE, ALL, COMBINED}
    [SerializeField] PlotMode plotMode = PlotMode.ALL;
    [SerializeField] RopeLinePoint[] ropeLinePoints;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        switch (plotMode)
        {
            case PlotMode.ALL:
                lineRenderer.positionCount = ropeLinePoints.Length * 2;
                break;
            case PlotMode.AVERAGE:
                lineRenderer.positionCount = ropeLinePoints.Length + 1;
                break;
            case PlotMode.COMBINED:
                lineRenderer.positionCount = (ropeLinePoints.Length * 3) - 1;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        for (int i = 0; i < ropeLinePoints.Length; i++)
        {
            ropeLinePoints[i].UpdatePoints();
        }

        switch (plotMode)
        {
            case PlotMode.ALL:
                PlotAllPoints();
                break;
            case PlotMode.AVERAGE:
                PlotPointsAtAverages();
                break;
            case PlotMode.COMBINED:
                PlotCombinedModePoints();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Place a point at the average between an end point and the next start point
    /// </summary>
    void PlotPointsAtAverages()
    {
        //set first point at origin of rope
        lineRenderer.SetPosition(0, ropeLinePoints[0].worldPosition);

        //place points at averages between an end point and the next start point
        for (int i = 0; i < ropeLinePoints.Length - 1; i++)
        {
            float avgX = (ropeLinePoints[i].segmentEndPoint.x + ropeLinePoints[i + 1].worldPosition.x) / 2f;
            float avgY = (ropeLinePoints[i].segmentEndPoint.y + ropeLinePoints[i + 1].worldPosition.y) / 2f;
            Vector2 avgPoint = new Vector2(avgX, avgY);
            lineRenderer.SetPosition(i + 1, avgPoint);
        }

        //place final point at end of rope
        lineRenderer.SetPosition(lineRenderer.positionCount-1, ropeLinePoints[ropeLinePoints.Length-1].segmentEndPoint);
    }

    /// <summary>
    /// Place a point at the start and end of each segment
    /// </summary>
    void PlotAllPoints()
    {
        for (int i = 0; i < ropeLinePoints.Length; i++)
        {
            lineRenderer.SetPosition(2 * i, ropeLinePoints[i].worldPosition);
            lineRenderer.SetPosition((2 * i) + 1, ropeLinePoints[i].segmentEndPoint);
        }
    }

    /// <summary>
    /// Place a point at the start of a segment and end, then also one at the average
    /// between an end and the next start
    /// </summary>
    void PlotCombinedModePoints()
    {
        //place start point for start, end and average to next, for each segment
        for (int i = 0; i < ropeLinePoints.Length - 1; i++)
        {
            lineRenderer.SetPosition(3 * i, ropeLinePoints[i].worldPosition);
            lineRenderer.SetPosition((3 * i) + 1, ropeLinePoints[i].segmentEndPoint);

            float avgX = (ropeLinePoints[i].segmentEndPoint.x + ropeLinePoints[i + 1].worldPosition.x) / 2f;
            float avgY = (ropeLinePoints[i].segmentEndPoint.y + ropeLinePoints[i + 1].worldPosition.y) / 2f;
            Vector2 avgPoint = new Vector2(avgX, avgY);
            lineRenderer.SetPosition((3 * i) + 2, avgPoint);
        }

        //place start and end point for last segment
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, ropeLinePoints[ropeLinePoints.Length - 1].worldPosition);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, ropeLinePoints[ropeLinePoints.Length - 1].segmentEndPoint);
    }
}
