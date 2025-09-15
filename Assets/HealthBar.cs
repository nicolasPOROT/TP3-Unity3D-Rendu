using UnityEngine;
public class Billboard : MonoBehaviour
{
    private Camera cam;
    void Start() => cam = Camera.main;
    void LateUpdate() => transform.LookAt(transform.position + cam.transform.forward); // Pour que la barre de vie soit toujours face à la caméra
}
