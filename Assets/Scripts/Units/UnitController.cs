using System;
using System.Collections;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [NonSerialized]
    public PathFind pathfinding;
    private int targetIndex;
    private Rigidbody rb;
    private Renderer renderer;
    private Color color;
    private bool found;
    Vector3 direction;

    public LayerMask unwalkableMask;
    public float speed;
    public bool leader;

    private void Awake()
    {
        pathfinding = GetComponent<PathFind>();
        rb = GetComponent<Rigidbody>();
        renderer = gameObject.GetComponent<Renderer>();
    }

    private void Start()
    {
        color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private void OnEnable()
    {
        LeaderController.onLead += TargetNodeChange;
        pathfinding.onPathFound += OnPathFound;
    }

    private void OnDisable()
    {
        LeaderController.onLead -= TargetNodeChange;
        pathfinding.onPathFound -= OnPathFound;
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

    public void OnPathFound()
    {
        Debug.Log("move" + " " + gameObject.name);

        targetIndex = 0;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }


    private void TargetNodeChange(Vector3 position)
    {
        if (leader)
            return;

        found = false;
        pathfinding.StartPathFinding(position); 
    }

    IEnumerator FollowPath()
    {
        if (pathfinding.Path.Length == 0)
        {
            StopCoroutine("FollowPath");
            yield return null;
        }

        Vector3 currentWaypoint = pathfinding.Path[0];
        while (true)
        {
           
            if (Vector3.Distance(currentWaypoint, transform.position) < 0.55f)
            {
                targetIndex++;
                if (targetIndex >= pathfinding.Path.Length)
                {
                    rb.velocity = Vector3.zero;
                    yield break;
                }
                currentWaypoint = pathfinding.Path[targetIndex];
            }
      
            direction = (currentWaypoint - transform.position).normalized;
            direction = Quaternion.Euler(0, -90, 0) * direction;
            direction.y = 0; 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.4f);

            rb.velocity = transform.right * speed;
            yield return new WaitForFixedUpdate();
        }
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
