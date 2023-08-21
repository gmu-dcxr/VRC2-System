using System;
using UnityEngine;

namespace VRC2.Authority
{
    public class GameObjectHook : AuthorityHook
    {

        public override void DisableP1()
        {
            gameObject.SetActive(false);
        }

        public override void DisableP2()
        {
            gameObject.SetActive(false);
        }

        public override void Default()
        {
            gameObject.SetActive(true);
        }
    }
}