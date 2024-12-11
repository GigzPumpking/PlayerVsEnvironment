using UnityEngine;

public class Web : EnemyBase
{
    [SerializeField] private float slowMultiplier = 0.7f;
    [SerializeField] private float range = 3f;

    private HeroKnight knight;
    private bool isSlowing = false;

    private void Start()
    {
        knight = FindFirstObjectByType<HeroKnight>();
    }

    private new void Update()
    {
        base.Update();

        if (knight == null)
        {
            return;
        }

        float distanceToKnight = Vector3.Distance(knight.transform.position, transform.position);

        if (distanceToKnight < range)
        {
            if (!isSlowing)
            {
                isSlowing = true;
                knight.AddSlowSource(slowMultiplier);
            }
        }
        else
        {
            if (isSlowing)
            {
                isSlowing = false;
                knight.RemoveSlowSource(slowMultiplier);
            }
        }
    }

    private void OnDestroy()
    {
        if (knight != null && isSlowing)
        {
            knight.RemoveSlowSource(slowMultiplier);
        }
    }
}
