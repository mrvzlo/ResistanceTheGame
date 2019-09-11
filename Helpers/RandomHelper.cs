using System;
using System.Collections.Generic;
using Resistance.Enums;
using Resistance.Helpers;

namespace Resistance
{
    public static class RandomHelper
    {
        public static Random Rand = new Random();

        public static List<Role> GetPlayerRoles(int playerCount)
        {
            var roles = new List<Role>();
            var redPlayerCount = Rules.GetRedPlayerCount(playerCount);
            for (; playerCount > 0; playerCount--)
            {
                var isRed = Rand.Next(playerCount) > redPlayerCount;
                roles.Add(isRed ? Role.Red : Role.Blue);
                if (isRed)
                    redPlayerCount--;
                //todo test
            }
            return roles;
        }

        public static int RandNum(int max) => Rand.Next(max);
    }
}