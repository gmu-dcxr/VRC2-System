using UnityEngine;
using System.Collections;

public class crane_animate2 : MonoBehaviour
{
    public Animator animator;
    public AnimatorControllerParameter animParam;

	public float rotateYaw;
    public float pitch;
    public float hook;

    public bool demoMode = false;

    float randomYawIncrease;
    float randomPitchIncrease;
    float randomHookIncrease;

    void Start()
    {
        randomYawIncrease = Random.Range( 1,10 );
        randomPitchIncrease = Random.Range(0.1f,1.0f);
        randomHookIncrease = Random.Range(0.1f,1.0f);
    }

	void Update ()
    {
        if ( demoMode )
        {
            rotateYaw += Time.deltaTime * randomYawIncrease;
            pitch = ((Mathf.Sin( Time.time * randomPitchIncrease ) * 100) + 100) /2.0f;
            hook = ((Mathf.Sin( Time.time * randomHookIncrease ) * 100) + 100) / 2.0f;
        }

        animator.SetFloat( "Rotate_YAW", Mathf.Abs( rotateYaw ) % 360  );
        animator.SetFloat( "pitch", pitch );
        animator.SetFloat( "hook", hook );
	}
}
