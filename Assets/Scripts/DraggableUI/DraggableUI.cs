using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Rigidbody2D rb;
    private bool isDragging = false;
    private Vector3 offset;

    public UISnapPoint snappedPoint = null;
    public string snapID;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        offset = transform.position - new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
        rb.velocity = Vector2.zero;

        foreach (var ui in FindObjectsOfType<DraggableUI>())
        {
            if (ui != this)
            {
                ui.rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        if (snappedPoint != null)
        {
            snappedPoint.Release();
            snappedPoint = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            Vector3 targetPos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;
            rb.MovePosition(targetPos);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        rb.gravityScale = 1;

        foreach (var ui in FindObjectsOfType<DraggableUI>())
        {
            ui.rb.bodyType = RigidbodyType2D.Dynamic;
        }

        UISnapPoint nearest = null;
        float minDist = float.MaxValue;
        foreach (var snap in FindObjectsOfType<UISnapPoint>())
        {
            float dist = Vector2.Distance(transform.position, snap.transform.position);
            if (dist < snap.snapRadius && snap.CanSnap(this) && dist < minDist)
            {
                minDist = dist;
                nearest = snap;
            }
        }

        if (nearest != null)
        {
            nearest.Snap(this);
            snappedPoint = nearest;
        }
    }

    public void OnSnapped(UISnapPoint point)
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.angularVelocity = 0;
        transform.rotation = Quaternion.identity;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
}
