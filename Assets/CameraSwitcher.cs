using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera playerCamera;
    public Camera bezierCamera;

    private bool isPlayerCamActive = true;

    void Start()
    {
        // Au d�but on active la cam�ra du joueur
        SetActiveCamera(true);
    }

    void Update()
    {
        // appuyer sur "C" pour changer
        if (Input.GetKeyDown(KeyCode.C))
        {
            isPlayerCamActive = !isPlayerCamActive;
            SetActiveCamera(isPlayerCamActive);
        }
    }

    void SetActiveCamera(bool usePlayer)
    {
        if (playerCamera != null) playerCamera.gameObject.SetActive(usePlayer);
        if (bezierCamera != null) bezierCamera.gameObject.SetActive(!usePlayer);
    }
}
