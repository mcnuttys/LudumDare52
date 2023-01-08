using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float zoomSpeed = 3;

    [SerializeField] private float minZoom = 8;
    [SerializeField] private float maxZoom = 16;

    private float zOffset = 20;
    private float rotX;
    private float rotY;

    public bool wander;
    public static CameraMovement instance;

    private float offset;

    private bool mouseDown;
    private float mouseTimer;

    void Start()
    {
        instance = this;

        offset = Random.Range(1000, 1000000);
    }

    void Update()
    {
        var toCenter = transform.position - target.position;

        var zoom = Input.mouseScrollDelta.y * zoomSpeed;
        zOffset -= zoom;

        zOffset = Mathf.Clamp(zOffset, minZoom, maxZoom);

        if (wander) zOffset = 30;

        transform.position = Vector3.Lerp(transform.position, toCenter.normalized * zOffset, 3 * Time.deltaTime);

        var dir = Vector2.zero;

        mouseDown = Input.GetMouseButton(0);
        if (mouseDown) mouseTimer += Time.deltaTime;
        if (!mouseDown) mouseTimer = 0;

        if (mouseTimer > 0.25f)
            dir += new Vector2(Input.GetAxis("Horizontal Test"), Input.GetAxis("Vertical Test"));

        dir += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (wander)
        {
            var p = Mathf.PerlinNoise((Time.time + offset) / 5, (Time.time + offset) / 5);
            var angle = p * Mathf.PI;

            dir.x += Mathf.Cos(angle);
            dir.y += Mathf.Sin(angle);
        }

        rotX = -dir.x * moveSpeed * Time.deltaTime;
        rotY = dir.y * moveSpeed * Time.deltaTime;

        //target.rotation = Quaternion.identity;
        transform.RotateAround(target.position, transform.right, rotY);
        transform.RotateAround(target.position, transform.up, rotX);


        Debug.DrawRay(target.position, transform.right, Color.green);
        Debug.DrawRay(target.position, transform.up, Color.red);
    }
}
