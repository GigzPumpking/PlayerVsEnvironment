using UnityEngine;

public class Web : EnemyBase
{

    [SerializeField] float drag = 0.7f;
    [SerializeField] float dragWalk = 0.7f;
    [SerializeField] float range = 3f;
    int timer = 0;

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        HeroKnight Knight = FindFirstObjectByType<HeroKnight>();

        Rigidbody2D tmpRigidbody2D = Knight.GetComponentInParent<Rigidbody2D>();
        Knight.m_speed = 4.0f;
        float tmpDist = Vector3.Distance(Knight.gameObject.transform.position, gameObject.transform.position);
        if (tmpDist < range)
        {
            tmpRigidbody2D.linearVelocity = tmpRigidbody2D.linearVelocity * drag;
            Knight.m_speed *= dragWalk;
            //print("dragging: " + tmpDist + ", " + range);
            //print("dragging: " + tmpRigidbody2D.linearVelocity);
        }
    }
    private void OnDestroy()
    {
        HeroKnight Knight = FindFirstObjectByType<HeroKnight>();
        Knight.m_speed = 4.0f;
    }
}