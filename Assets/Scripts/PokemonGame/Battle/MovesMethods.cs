using PokemonGame.General;

namespace PokemonGame.Battle
{
    using Global;
    using UnityEngine;
    using ScriptableObjects;

    /// <summary>
    /// Contains all of the logic for every move
    /// </summary>
    [CreateAssetMenu(fileName = "New Moves Methods", menuName = "All/New Moves Methods")]
    public class MovesMethods : ScriptableObject
    {
        private int CalculateDamage(Move move, Battler battlerThatUsed, Battler battlerBeingAttacked)
        {
            //Checking to see if the move is capable of hitting the opponent battler
            foreach (var hType in move.type.cantHit)
            {
                if (hType == battlerBeingAttacked.primaryType || hType == battlerBeingAttacked.secondaryType)
                {
                    Debug.Log(move.type + " can't hit that battler");
                    return 0;
                }
            }

            float type = 1;

            //Calculating type disadvantages
            foreach (var weakType in move.type.weakAgainst)
            {
                if (weakType == battlerBeingAttacked.primaryType)
                {
                    type /= 2;
                }
                if (weakType == battlerBeingAttacked.secondaryType)
                {
                    type /= 2;
                }
            }

            //Calculating type advantages
            foreach (var strongType in move.type.strongAgainst)
            {
                if (strongType == battlerBeingAttacked.primaryType)
                {
                    type *= 2;
                }
                if (strongType == battlerBeingAttacked.secondaryType)
                {
                    type *= 2;
                }
            }

            //Failsafe
            if (type > 4)
                type = 4;
            if (type < .25f)
                type = .25f;

            //STAB =  Same type attack bonus
            int stab = 1;
            if (move.type == battlerThatUsed.primaryType)
            {
                stab = 2;
            }

            //Damage calculation is correct (took me way to long to get it right) source: https://bulbapedia.bulbagarden.net/wiki/Damage#Generation_II
            int damage = move.category == MoveCategory.Physical
                ? Mathf.RoundToInt(((2 * battlerThatUsed.level / 5 + 2) * move.damage *
                    (battlerThatUsed.attack / battlerBeingAttacked.defense) / 50 + 2) * stab * type)
                : Mathf.RoundToInt(((2 * battlerThatUsed.level / 5 + 2) * move.damage *
                    (battlerThatUsed.specialAttack / battlerBeingAttacked.specialDefense) / 50 + 2) * stab * type);

            int randomness = Mathf.RoundToInt(Random.Range(.8f * damage, damage * 1.2f));
            damage = randomness;

            return damage;
        }
        
        public void Toxic(MoveMethodEventArgs e)
        {
            e.target.statusEffect = Registry.GetStatusEffect("Poisoned");
            Debug.Log("Used Toxic on " + e.target.name);
        }

        public void Ember(MoveMethodEventArgs e)
        {
            Debug.Log("Used Ember on " + e.target.name);
            int damage = CalculateDamage(e.move, e.attacker, e.target);
            e.target.TakeDamage(damage);
        }

        public void RazorLeaf(MoveMethodEventArgs e)
        {
            Debug.Log("Used Razor Leaf on " + e.target.name);
            int damage = CalculateDamage(e.move, e.attacker, e.target);
            e.target.TakeDamage(damage);
        }

        public void Tackle(MoveMethodEventArgs e)
        {
            Debug.Log("Used Tackle on " + e.target.name);
            int damage = CalculateDamage(e.move, e.attacker, e.target);
            e.target.TakeDamage(damage);
        }
    }   
}