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
    public partial class ManaAuraAction
    {
        private float m_ActionStartTimeClient;

        public override bool OnStartClient(ClientCharacter clientCharacter)
        {
            base.OnStartClient(clientCharacter);
            m_ActionStartTimeClient = Time.time;
            m_SpawnedGraphics = InstantiateSpecialFXGraphics(clientCharacter.transform, true);
            return true;
        }

        public override void CancelClient(ClientCharacter clientCharacter)
        {
            // if we had any special target graphics, tell them we're done
            if (m_SpawnedGraphics != null)
            {
                foreach (var spawnedGraphic in m_SpawnedGraphics)
                {
                    if (spawnedGraphic)
                    {
                        spawnedGraphic.Shutdown();
                    }
                }
            }
        }

        public override bool OnUpdateClient(ClientCharacter clientCharacter)
        {
            return Time.time < (m_ActionStartTimeClient + Config.EffectDurationSeconds);
        }
    }
}
