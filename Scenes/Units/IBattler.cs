using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledTowerDefenceGame.Scenes.Effects;

namespace UntitledTowerDefenceGame.Scenes.Units
{
    public interface IBattler
    {
        float Damage { get; set; }
        float Health { get; set; }
        float Armor { get; set; }
        float Speed { get; set; }
        int AttackCooldown { get; set; }
        int CurrentAttackCooldown { get; set; }
        bool InCombat { get; set; }
        bool IsAlive { get; set; } //Maybe also IsAttackable? for the case of banishment and other things
        IBattler Target { get; set; }
        List<IEffect> CurrentEffects { get; set; }
        bool EnterCombat(IBattler newTarget);
        void LeaveCombat();
        bool AttackTarget();
        bool TakeDamage(float damageDone); //should there be damage types?
        bool TakeDamage(float damageDone, IBattler attacker);
        void TakeEffects(List<IEffect> effects);
    }
}
