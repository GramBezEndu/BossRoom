using Unity.BossRoom.Gameplay.GameplayObjects;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.Actions
{
    [CreateAssetMenu(menuName = "BossRoom/Actions/Mana Aura Action")]
    public partial class ManaAuraAction : Action
    {
        private float m_ActionStartTime;

        private float m_LastRestoreTime;

        public override bool OnStart(ServerCharacter serverCharacter)
        {
            m_ActionStartTime = Time.time;
            serverCharacter.serverAnimationHandler.NetworkAnimator.SetTrigger(Config.Anim);
            serverCharacter.clientCharacter.RecvDoActionClientRPC(Data);
            return true;
        }

        public override void Reset()
        {
            base.Reset();

            m_ActionStartTime = 0;
            m_ActionStartTimeClient = 0;
            m_LastRestoreTime = 0;
            m_SpawnedGraphics = null;
        }

        public override bool OnUpdate(ServerCharacter serverCharacter)
        {
            if (m_LastRestoreTime + Config.TickInterval < Time.time)
            {
                RestoreManaTick(serverCharacter);
            }

            return Time.time < (m_ActionStartTime + Config.EffectDurationSeconds);
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
