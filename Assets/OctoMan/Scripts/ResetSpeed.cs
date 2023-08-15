/*This scripts needs to be on the excavator itself. The functions are called
 * from animation events in the bigarm, smallarm, shovel animations to prevent overflow
 * on frame 1 in the animation set the corresponding value to 0
 * on last frame in the animation set the corresponding value to 2
 */

using UnityEngine;
using System.Collections;

public class ResetSpeed : MonoBehaviour {

	public ExcavatorScript excav;

	public void BigArmPosition(int pos)//0 = down, 1 = middle, 2 = up
	{
		excav.anim.SetInteger("BigArmPosition", pos);
		excav.anim.SetFloat("BigArmSpeed", 0f);
	}
		
	public void SmallArmPosition(int pos)//0 = down, 1 = middle, 2 = up
	{
		excav.anim.SetInteger("SmallArmPosition", pos);
		excav.anim.SetFloat("SmallArmSpeed", 0f);
	}

	public void ShovelPosition(int pos)//0 = down, 1 = middle, 2 = up
	{
		excav.anim.SetInteger("ShovelPosition", pos);
		excav.anim.SetFloat("ShovelSpeed", 0f);
	}
}
