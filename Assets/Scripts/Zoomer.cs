using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Zoomer : MonoBehaviour
{
    [SerializeField]
    private InputAction rightClick;
    private Camera mainCamera;
    private Vector3 camLoc;
    private Vector3 zoomLoc;
    private int zoomDir;
    private float timer;
    private float zoomTime = 2.0f;
    private void Awake()
    {
        mainCamera = Camera.main;
        camLoc = new Vector3(0, 0, 14.5f);
    }
    private void OnEnable()
    {
        rightClick.Enable();
        rightClick.performed += rightClicked;
    }
    private void OnDisable()
    {
        rightClick.performed -= rightClicked;
        rightClick.Disable();
    }

    private void rightClicked(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject.layer==3)
            {
                zoomDir = hit.collider.gameObject.GetComponent<ZoomLayerObj>().zoomDir;
                hit.collider.gameObject.GetComponent<ZoomLayerObj>().zoomDir *= -1;
                StartCoroutine(ZoomIn(hit.collider.gameObject));
            }
        }
    }

    private IEnumerator ZoomIn(GameObject clickedObject)
    {
       // camLoc = mainCamera.transform.position;
        zoomLoc = clickedObject.transform.position;
        zoomLoc.z -= 15;
        timer = 0.0f;
        while (timer < zoomTime) {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            if (zoomDir == 1) { mainCamera.transform.position = Vector3.Lerp(camLoc, zoomLoc, timer / zoomTime); }
            else { mainCamera.transform.position = Vector3.Lerp(zoomLoc, camLoc, timer / zoomTime); }
        }
        yield return null;
    }
}
