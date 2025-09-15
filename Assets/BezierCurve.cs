using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{
    public Transform[] controlPoints; // points de contrôle (3 pour quadratique, 4 pour cubique)
    public int segmentCount = 50;     // nombre de segments de la courbe
    private LineRenderer lineRenderer;

    public enum BezierType { Quadratic, Cubic }
    public BezierType bezierType = BezierType.Quadratic;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segmentCount + 1;
    }

    void Update()
    {
        DrawCurve();
    }

    void DrawCurve()
    {
        lineRenderer.positionCount = segmentCount + 1;

        for (int i = 0; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            Vector3 position = Vector3.zero;

            if (bezierType == BezierType.Quadratic && controlPoints.Length >= 3)
            {
                position = CalculateQuadraticBezierPoint(t, controlPoints[0].position, controlPoints[1].position, controlPoints[2].position);
            }
            else if (bezierType == BezierType.Cubic && controlPoints.Length >= 4)
            {
                position = CalculateCubicBezierPoint(t,
                    controlPoints[0].position,
                    controlPoints[1].position,
                    controlPoints[2].position,
                    controlPoints[3].position);
            }

            lineRenderer.SetPosition(i, position);
        }
    }

    // Bézier quadratique (3 points de contrôle)
    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }

    // Bézier cubique (4 points de contrôle)
    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        return (u * u * u * p0) +
               (3 * u * u * t * p1) +
               (3 * u * t * t * p2) +
               (t * t * t * p3);
    }

    // Gizmos pour voir les points et les lignes de contrôle dans l’éditeur
    void OnDrawGizmos()
    {
        if (controlPoints == null) return;

        Gizmos.color = Color.red;
        foreach (var cp in controlPoints)
        {
            if (cp != null)
                Gizmos.DrawSphere(cp.position, 0.1f);
        }

        Gizmos.color = Color.gray;
        for (int i = 0; i < controlPoints.Length - 1; i++)
        {
            if (controlPoints[i] != null && controlPoints[i + 1] != null)
                Gizmos.DrawLine(controlPoints[i].position, controlPoints[i + 1].position);
        }
    }
}
