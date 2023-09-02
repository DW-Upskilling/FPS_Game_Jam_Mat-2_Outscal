using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Outscal.UnityAdvanced.Mat2.Utils;
using Outscal.UnityAdvanced.Mat2.Components.Character.Player;

namespace Outscal.UnityAdvanced.Mat2.Components.Character.Enemy
{
    public class EnemyRadar : MonoBehaviour
    {
        [SerializeField]
        private Animator stateMachine;

        [SerializeField]
        private EnemyPointOfView enemyPointOfView;

        public GameObject ColliderGameObject { get; set; }

        private int chaseId;
        private float cooldown;

        private void Awake()
        {
            if (enemyPointOfView == null)
                throw new UnassignedReferenceException("Assign enemyPointOfView gameObject");
            chaseId = Animator.StringToHash("Chase");
            cooldown = Constants.DefaultStateCooldown;
        }

        private void Update()
        {
            cooldown -= Time.deltaTime;
        }

        private void OnTriggerStay(Collider collider)
        {
            PlayerView playerView = collider.gameObject.GetComponent<PlayerView>();
            if(playerView != null && cooldown <= 0)
            {
                ColliderGameObject = collider.gameObject;

                stateMachine.SetTrigger(chaseId);
                enemyPointOfView.gameObject.SetActive(true);

                cooldown = Constants.DefaultStateCooldown;
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.Equals(ColliderGameObject))
            {
                enemyPointOfView.gameObject.SetActive(false);

                cooldown = Constants.DefaultStateCooldown;
            }
        }
    }
}
