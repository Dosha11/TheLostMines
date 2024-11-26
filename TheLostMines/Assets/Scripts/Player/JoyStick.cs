using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private float _maxDistance;
    [SerializeField] private bool _rotation;

    [SerializeField] private Vector3 _direction;
    [SerializeField] Vector3 worldDirection;
    [SerializeField] Player player;
    [SerializeField] float angle;



    private void Start()
    {
        _startPosition = transform.position;
        player = FindObjectOfType<Player>();

    }

    public void OnDrag(PointerEventData eventData)
    {
        _direction = (Vector3)eventData.position - _startPosition;
        transform.position = _startPosition + Vector3.ClampMagnitude(_direction, _maxDistance);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _direction = Vector3.zero;
        worldDirection = Vector3.zero;
        transform.position = _startPosition;
    }

    private void Update()
    {
        if (player)
        {
            if (_rotation)
            {
                angle = Mathf.Atan2(_direction.x, _direction.y) * 180 / Mathf.PI;
                player.GetDirectionRotate(angle);
            }
            else
            {
                worldDirection = player.transform.TransformDirection(new Vector3(_direction.x, 0, _direction.y));
                player.GetDirectionMove(worldDirection);
            }
        }
        else
        {
            player = FindObjectOfType<Player>();
        }
    }
}