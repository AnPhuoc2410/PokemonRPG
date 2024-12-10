using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Battles
{
    public enum BattleState
    {
        Start,
        PlayerAction,
        PlayerMove,
        EnemyMove,
        Busy,
        PartyScreen,
        Bag,
        BattleOver
    }
}
