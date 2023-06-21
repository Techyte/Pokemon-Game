using UnityEngine;

namespace PokemonGame.General
{
    public class ExperienceCalculator : MonoBehaviour
    {
        public int GetExperienceFromDefeatingBattler(Battler defeated, bool trainer, int noOfBattlersParticipated)
        {
            // Calculations from: https://bulbapedia.bulbagarden.net/wiki/Experience#Experience_gain_in_battle
            
            int exp = 0;

            double a = trainer ? 1.5 : 1;

            // luckey egg
            double e = 1;

            double s = noOfBattlersParticipated;

            double b = defeated.source.baseExpYield;

            double L = defeated.level;

            exp = (int)(b * L / 7 * 1 / s * e * a);

            return exp;
        }
    }   
}