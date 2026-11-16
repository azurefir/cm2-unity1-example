using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int pointValue = 10;
    [SerializeField] private float spinSpeed = 90f;
    [SerializeField] private float bobHeight = 0.25f;
    [SerializeField] private float bobSpeed = 2f;

    private Vector3 startPosition;
    private bool collected;
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPosition + Vector3.up * offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected ||! other.CompareTag("Player")) return;

        collected = true;

        Destroy(gameObject);
    }
}
