using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class DragAndDrop2D : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;

    void OnEnable()
    {
        // Active le support tactile étendu nécessaire pour la nouvelle API Touch
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Récupère la position actuelle du pointeur (souris ou premier doigt actif)
        Vector2 currentScreenPos;
        bool isPressedDown = false;
        bool isHeld = false;
        bool isReleased = false;

        // 1. Gestion Tactile (New Input System EnhancedTouch)
        if (Touch.activeTouches.Count > 0)
        {
            Touch primaryTouch = Touch.activeTouches[0];
            currentScreenPos = primaryTouch.screenPosition;

            isPressedDown = primaryTouch.phase == UnityEngine.InputSystem.TouchPhase.Began;
            isHeld = primaryTouch.phase == UnityEngine.InputSystem.TouchPhase.Moved || primaryTouch.phase == UnityEngine.InputSystem.TouchPhase.Stationary;
            isReleased = primaryTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended || primaryTouch.phase == UnityEngine.InputSystem.TouchPhase.Canceled;
        }
        // 2. Fallback Souris (si aucun toucher actif)
        else if (Mouse.current != null)
        {
            currentScreenPos = Mouse.current.position.ReadValue();

            isPressedDown = Mouse.current.leftButton.wasPressedThisFrame;
            isHeld = Mouse.current.leftButton.isPressed;
            isReleased = Mouse.current.leftButton.wasReleasedThisFrame;
        }
        else
        {
            return;
        }

        // --- Logique du Drag & Drop ---

        // Début du clic/touché
        if (isPressedDown)
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(currentScreenPos), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(currentScreenPos);
                worldPos.z = transform.position.z;
                offset = transform.position - worldPos;
            }
        }

        // Maintien et déplacement
        if (isDragging && isHeld)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(currentScreenPos);
            worldPos.z = transform.position.z;
            transform.position = worldPos + offset;
        }

        // Relâchement
        if (isDragging && isReleased)
        {
            isDragging = false;
        }
    }
}