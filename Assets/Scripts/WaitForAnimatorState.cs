using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class WaitForAnimatorState
    {
        public static IEnumerator Do(Animator animator, string stateName)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                yield return null;
            }
        }
    }
}
