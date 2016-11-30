using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeepGrinding
{
    class Player
    {
        private int health;
        private int attack;
        private int defense;
        private int speed;

        public void setStats(int h, int a, int s, int d)
        {
            health = h;
            attack = a;
            speed = s;
            defense = d;
        }
        public void setHealth(int h)
        {
            health = h;
        }
        public void setSpeed(int s)
        {
            speed = s;
        }
        public void setAttack(int a)
        {
            attack = a;
        }
        public void setDefense(int d)
        {
            defense = d;
        }
        public void takeDamage(int t)
        {
            health -= t;
        }
        public int getSpeed()
        {
            return speed;
        }
        public int getAttack()
        {
            return attack;
        }
        public int getDefense()
        {
            return defense;
        }
        public int getHealth()
        {
            return health;
        }
    }
}
