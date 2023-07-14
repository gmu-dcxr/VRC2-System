using UnityEngine;
using System.Collections;

public class crane_animate1 : MonoBehaviour
{
    public Animator animator;
    public AnimatorControllerParameter animParam;

	public float rotateYaw;
    public float dolly;
    public float hook;

    public bool demoMode = false;

    float randomYawIncrease;
    float randomDollyIncrease;
    float randomHookIncrease;

    void Start()
    {
        randomYawIncrease = Random.Range( 1,10 );
        randomDollyIncrease = Random.Range(0.1f,1.0f);
        randomHookIncrease = Random.Range(0.1f,1.0f);
    }

	void Update ()
    {
        if ( demoMode )
        {
            rotateYaw += Time.deltaTime * randomYawIncrease;
            dolly = ((Mathf.Sin( Time.time * randomDollyIncrease ) * 100) + 100) /2.0f;
            hook = ((Mathf.Sin( Time.time * randomHookIncrease ) * 100) + 100) / 2.0f;
        }

        animator.SetFloat( "Rotate_YAW", Mathf.Abs( rotateYaw ) % 360  );
        animator.SetFloat( "dolly", dolly );
        animator.SetFloat( "hook", hook );
	}
}
