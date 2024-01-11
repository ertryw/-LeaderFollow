using System.Collections;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField]
    private LayerMask unwalkableMask;
    [SerializeField]
    public float speed;

    private bool leader;
    private bool found;
    private bool coroutineRunning = false;
    private int targetIndex;

    private UnitBase unit;
    private Rigidbody rb;
    private Color color;
    private new Renderer renderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        color = renderer.material.color;
        unit = GetComponent<UnitBase>();
    }

    private void OnEnable()
    {
        LeaderController.onLead += TargetNodeChange;
        unit.AddPathFound(OnPathFound);
    }

    private void OnDisable()
    {
        LeaderController.onLead -= TargetNodeChange;
        unit.RemovePathFound(OnPathFound);
    }

    public void SwitchToDefault()
    {
        leader = false;
        found = false;
        renderer.material.color = color;
    }

    public void SwitchToLeader()
    {
        leader = true;
        found = false;
        renderer.material.color = Color.yellow;
    }

    public void OnPathFound(Vector3[] path)
    {
        if (coroutineRunning || path.Length == 0)
        {
            StopAllCoroutines();
            coroutineRunning = false;
        }

        if (path.Length == 0)
            return;
        
        IEnumerator followPath = FollowPath(path);
        StartCoroutine(followPath);         
    }


    private void TargetNodeChange(Vector3 target)
    {
        if (leader)
            return;

        found = false;
        unit.StartPathFinding(transform.position, target); 
    }

    IEnumerator FollowPath(Vector3[] path)
    {
        targetIndex = 0;
        coroutineRunning = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 currentWaypoint = path[0];

        while (true)
        {
           
            if (Vector3.Distance(currentWaypoint, transform.position) < 0.55f)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    rb.velocity = Vector3.zero;
                    break;
                }
                currentWaypoint = path[targetIndex];
            }

            Vector3 direction = (currentWaypoint - transform.position).normalized;

            direction = Quaternion.Euler(0, -90, 0) * direction;
            direction.y = 0; 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.4f);

            rb.velocity = transform.right * speed;
            yield return new WaitForFixedUpdate();
        }

        coroutineRunning = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (leader)
            return;

        UnitController colliderController = collision.gameObject.GetComponent<UnitController>();

        if (colliderController == null)
            return;

        if (colliderController.leader == true || colliderController.found)
            found = true;
         
    }

    private void FixedUpdate()
    {
        if (rb.angularVelocity.magnitude > 0)
        {
            rb.angularVelocity *= 0.1f;
        }

        if (found)
        {
            rb.velocity *= 0.1f;
        }
    }
}
