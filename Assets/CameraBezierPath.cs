using UnityEngine;

public class CameraBezierFollow : MonoBehaviour
{
    private BezierCurve bezierCurve;  // référence au script BezierCurve du parent
    public float duration = 10f;      // temps du trajet complet en secondes
    private float timer = 0f;

    void Start()
    {
        // Récupère automatiquement le BezierCurve du parent
        bezierCurve = GetComponentInParent<BezierCurve>();
        if (bezierCurve == null)
        {
            Debug.LogError("Aucun BezierCurve trouvé sur le parent !");
        }
    }

    void Update()
    {
        if (bezierCurve == null || bezierCurve.controlPoints == null) return;

        // Incrémente le temps
        timer += Time.deltaTime;
        float t = (timer % duration) / duration;

        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.forward;

        if (bezierCurve.bezierType == BezierCurve.BezierType.Quadratic && bezierCurve.controlPoints.Length >= 3)
        {
            pos = QuadraticBezier(t,
                bezierCurve.controlPoints[0].position,
                bezierCurve.controlPoints[1].position,
                bezierCurve.controlPoints[2].position);

            dir = QuadraticBezierTangent(t,
                bezierCurve.controlPoints[0].position,
                bezierCurve.controlPoints[1].position,
                bezierCurve.controlPoints[2].position);
        }
        else if (bezierCurve.bezierType == BezierCurve.BezierType.Cubic && bezierCurve.controlPoints.Length >= 4)
        {
            pos = CubicBezier(t,
                bezierCurve.controlPoints[0].position,
                bezierCurve.controlPoints[1].position,
                bezierCurve.controlPoints[2].position,
                bezierCurve.controlPoints[3].position);

            dir = CubicBezierTangent(t,
                bezierCurve.controlPoints[0].position,
                bezierCurve.controlPoints[1].position,
                bezierCurve.controlPoints[2].position,
                bezierCurve.controlPoints[3].position);
        }

        // Déplace et oriente la caméra
        transform.position = pos;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    // Quadratique
    Vector3 QuadraticBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }

    Vector3 QuadraticBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (2 * (1 - t) * (p1 - p0)) + (2 * t * (p2 - p1));
    }

    // Cubique
    Vector3 CubicBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        return (u * u * u * p0) +
               (3 * u * u * t * p1) +
               (3 * u * t * t * p2) +
               (t * t * t * p3);
    }

    Vector3 CubicBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        return (-3 * u * u * p0) +
               (3 * u * u - 6 * u * t) * p1 +
               (6 * u * t - 3 * t * t) * p2 +
               (3 * t * t * p3);
    }
}
