using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public class DragDropper : MonoBehaviour
{
    [SerializeField]
    private InputAction leftClick;
    private Camera mainCamera;
    
    private Vector3 velocity = Vector3.zero;
    private float mouseDragPhysicsSpeed = 10.0f;
    private float mouseDragSpeed = 0.1f;

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        leftClick.Enable();
        leftClick.performed += leftClicked;
    }
    private void OnDisable()
    {   
        leftClick.performed -= leftClicked;
        leftClick.Disable();
    }

    private void leftClicked(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider != null && (hit.collider.gameObject.CompareTag("Pollutant"))) {
                if (hit.collider!= null) {
                    StartCoroutine(DragUpdate(hit.collider.gameObject));
                }
            }
        }
    }

    private IEnumerator DragUpdate(GameObject clickedObject) {
        float initialDistance = Vector3.Distance(clickedObject.transform.position, mainCamera.transform.position);
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        while (leftClick.ReadValue<float>() != 0) {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            //not using rigid body
            if (rb != null)
            {
                Vector2 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
                rb.velocity = direction * mouseDragPhysicsSpeed;
                yield return waitForFixedUpdate;
            }
            //using rigid body
            else {
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, ray.GetPoint(initialDistance),
                    ref velocity, mouseDragSpeed);
                yield return null;
            }
        }
    }

}
