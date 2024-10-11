using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDropMaster : MonoBehaviour
{
    [Header("Y Axis Settings")]
    [SerializeField] private float m_YGrabbed = 1f;
    [SerializeField] private float m_YGrounded = 0f;
    [SerializeField] private float m_YAboveObject = 0.25f;

    [Header("Rotation Settings")]
    [SerializeField] private Vector3 m_RotationAngle;

    private Transform m_SelectedObject;
    private PuzzlePiece m_PuzzlePiece;

    private void Update()
    {
        // Pick up object
        if (InputManager.Instance.LeftClick.Tap)  // Detect when left click starts
        {
            if (m_SelectedObject == null)
            {
                RaycastHit hit = CastRay();

                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("Drag")) return;

                    m_SelectedObject = hit.collider.gameObject.transform;
                    m_PuzzlePiece = m_SelectedObject.GetComponent<PuzzlePiece>();

                    if (!m_PuzzlePiece.CanDrag())
                    {
                        m_PuzzlePiece = null;
                        m_SelectedObject = null;
                    }
                    else
                    {
                        Cursor.visible = false;
                    }
                }
            }
        }

        if (m_SelectedObject == null || m_PuzzlePiece == null) return;

        // Drag object while holding
        if (InputManager.Instance.LeftClick.Hold)
        {
            Drag();

            // Rotate object
            if (InputManager.Instance.RightClick.Tap)
            {
                m_SelectedObject.rotation = Quaternion.Euler(new Vector3(
                    m_SelectedObject.rotation.eulerAngles.x + m_RotationAngle.x,
                    m_SelectedObject.rotation.eulerAngles.y + m_RotationAngle.y,
                    m_SelectedObject.rotation.eulerAngles.z + m_RotationAngle.z));
            }
        }

        // Release the object
        if (InputManager.Instance.LeftClick.Release)
        {
            Release();

            m_PuzzlePiece.Snap();

            m_PuzzlePiece = null;
            m_SelectedObject = null;
            Cursor.visible = true;
        }
    }

    private RaycastHit CastRay()
    {
        Vector3 l_screenMousePosFar = new Vector3(InputManager.Instance.MousePosition.x, InputManager.Instance.MousePosition.y, Camera.main.farClipPlane);
        Vector3 l_screenMousePosNear = new Vector3(InputManager.Instance.MousePosition.x, InputManager.Instance.MousePosition.y, Camera.main.nearClipPlane);

        Vector3 l_worldMousePosFar = Camera.main.ScreenToWorldPoint(l_screenMousePosFar);
        Vector3 l_worldMousePosNear = Camera.main.ScreenToWorldPoint(l_screenMousePosNear);

        RaycastHit l_hit;
        Physics.Raycast(l_worldMousePosNear, l_worldMousePosFar - l_worldMousePosNear, out l_hit);

        return l_hit;
    }

    private void Drag()
    {
        Vector3 l_worldPosition = MouseToWorldObjectPosition();

        m_SelectedObject.position = new Vector3(l_worldPosition.x, m_YGrabbed, l_worldPosition.z);
    }

    private void Release()
    {
        Vector3 l_worldPosition = MouseToWorldObjectPosition();

        // Perform raycast downwards from the object's current position to detect if there is something below
        RaycastHit l_hit;
        Vector3 l_newPosition;

        // Cast a ray downward to check if there's any object beneath the selected object
        if (Physics.Raycast(m_SelectedObject.position, Vector3.down, out l_hit, Mathf.Infinity) && l_hit.collider.CompareTag("Drag"))
        {
            // If something is below, set the Y position to the default value
            l_newPosition = new Vector3(l_worldPosition.x, m_YAboveObject, l_worldPosition.z);
        }
        else
        {
            // If nothing is below, allow the object to follow the mouse position using the specified Y value
            l_newPosition = new Vector3(l_worldPosition.x, m_YGrounded, l_worldPosition.z);
        }

        m_SelectedObject.position = l_newPosition;
    }

    private Vector3 MouseToWorldObjectPosition()
    {
        Vector3 l_position = new Vector3(InputManager.Instance.MousePosition.x, InputManager.Instance.MousePosition.y,
            Camera.main.WorldToScreenPoint(m_SelectedObject.position).z);

        return Camera.main.ScreenToWorldPoint(l_position);
    }

    public Vector3 GetRotationAngle()
    {
        return m_RotationAngle;
    }
}