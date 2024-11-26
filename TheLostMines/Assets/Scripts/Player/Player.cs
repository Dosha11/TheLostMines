using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public float speed;
    public float speedRotation;
    [SerializeField] CharacterController characterController;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _directionRotate;
    [SerializeField] GameObject camera;
    [SerializeField] Item selectItem;


    public Vector3 InputMove;
    public float InputRotate;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>().gameObject;


        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        Move();
        Rotation();
        Determinant();


    }

    public void Determinant()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(Camera.main.ScreenToViewportPoint(Input.mousePosition));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (hit.collider.gameObject.GetComponent<MarkerParent>())
                    {
                        hit.collider.gameObject.GetComponent<MarkerParent>().CollisionOre();
                    }
                    else if (hit.collider.gameObject.GetComponent<CaveView>())
                    {
                        hit.collider.gameObject.GetComponent<CaveView>().Touch();
                    }
                    else if (hit.collider.gameObject.GetComponent<OreMarker>())
                    {
                        hit.collider.gameObject.GetComponent<OreMarker>().Touch();
                    }
                }
            }
        }
    }

    public void GetDirectionMove(Vector3 direction)
    {
        _direction = direction;
    }
    public void GetDirectionRotate(float angle)
    {
        if ((angle < -45 && angle > -135) || (angle > 45 && angle < 135))
        {
            transform.Rotate(transform.up, angle * speedRotation * Time.deltaTime);

        }
        else if ((angle >= -45) && (angle <= 45) && angle != 0)
        {
            if (camera.transform.localRotation.x > -0.3f)
            {
                camera.transform.Rotate(Vector3.right, -50 * speedRotation * Time.deltaTime);

            }
        }
        else if ((angle <= -135) && (angle > -180) || (angle >= 135) && (angle < 180))
        {
            if (camera.transform.localRotation.x < 0.4f)
            {
                camera.transform.Rotate(Vector3.right, 50 * speedRotation * Time.deltaTime);
            }
        }
    }

    public void Move()
    {
        characterController.SimpleMove(_direction * speed * Time.deltaTime);
    }
    public void Rotation()
    {
        transform.Rotate(transform.up, _directionRotate * speedRotation * Time.deltaTime);

    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void ChangePosition(Vector3 position)
    {
        transform.position = position;
    }
}
