using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.BossRoom.Gameplay.GameplayObjects;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using Unity.BossRoom.VisualEffects;
using UnityEngine;
using System.Linq;

namespace Unity.BossRoom.Gameplay.Actions
{
    [CreateAssetMenu(menuName = "BossRoom/Actions/Mana Aura Action")]
    public partial class ManaAuraAction : Action
    {
        private float m_ActionStartTime;

        private float m_LastRestoreTime;

        /// <summary>
        /// List of active special graphics playing on the target.
        /// </summary>
        private List<SpecialFXGraphic> m_SpawnedGraphics = null;

        public override bool OnStart(ServerCharacter serverCharacter)
        {
            m_ActionStartTime = Time.time;

            serverCharacter.serverAnimationHandler.NetworkAnimator.SetTrigger(Config.Anim);
            serverCharacter.clientCharacter.RecvDoActionClientRPC(Data);
            return true;
        }

        public override void End(ServerCharacter serverCharacter)
        {
            base.End(serverCharacter);
        }

        public override bool OnUpdate(ServerCharacter serverCharacter)
        {
            if (m_LastRestoreTime + 0.05f < Time.time)
            {
                RestoreManaTick(serverCharacter);
            }

            return Time.time < (m_ActionStartTime + Config.EffectDurationSeconds);
        }

        public override void Reset()
        {
            base.Reset();
            m_ActionStartTime = 0;
            m_ActionStartTimeClient = 0;
            m_LastRestoreTime = 0;
            m_SpawnedGraphics = null;
        }

        private void RestoreManaTick(ServerCharacter serverCharacter)
        {
            m_LastRestoreTime = Time.time;

            var colliders = Physics.OverlapSphere(serverCharacter.physicsWrapper.Transform.position, Config.Radius, LayerMask.GetMask("PCs"));
            for (var i = 0; i < colliders.Length; i++)
            {
                var ally = colliders[i].GetComponent<ManaReceiver>();
                if (ally != null)
                {
                    ally.ReceiveMana(Config.Amount);
                }
            }
        }
    }
}
