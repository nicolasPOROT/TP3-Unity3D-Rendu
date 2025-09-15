using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Cible")]
    public Transform player;            
    public Transform cam;

    [Header("Position & zoom")]
    public Vector3 offset = new Vector3(0, 2, -5);
    public float mouseSensitivity = 3f;
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    [Header("Angles")]
    public float minPitch = -20f;
    public float maxPitch = 60f;

    private float distance;
    private float yaw;
    private float pitch = 20f;
    private Rigidbody playerRb;

    void Start()
    {
        if (player == null) Debug.LogError("CameraController: assigne la Transform du PlayerRig (player).");
        playerRb = player ? player.GetComponent<Rigidbody>() : null;

        if (player != null) yaw = player.eulerAngles.y;

        distance = offset.magnitude;

        if (cam == null && Camera.main != null)
            cam = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        HandleZoom();
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 direction = rotation * Vector3.back * distance;

        transform.position = player.position + direction;
        
        transform.rotation = rotation;

        if (cam != null)
            cam.LookAt(player.position + Vector3.up * 1.5f);
    }

    void FixedUpdate()
    {
        
        if (playerRb != null && Input.GetMouseButton(1))
        {
            Quaternion target = Quaternion.Euler(0f, yaw, 0f);
            playerRb.MoveRotation(target);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > Mathf.Epsilon)
        {
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minZoom, maxZoom);
        }
    }
}
