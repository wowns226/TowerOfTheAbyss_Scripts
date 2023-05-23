// Copyright (C) <2023>  
//     Authors : Shin YongUk <dyddyddnr@naver.com>
//     Lim jaejun <wowns226@naver.com>
//  
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//  
//     You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>

namespace ProjectL
{
    public enum RepairDroneState
    {
        Repair = 0,
        Buff = 1,
        DeBuff = 2,
    }

    public class RepairDrone : Drone
    {
        [SettingValue]
        protected float fixRate;
        public float FixRate => fixRate;

        private UnitState<Drone>[] repairDroneState = { new Repair<Drone>(new Move<Drone>(new SearchRepairing<Drone>(new Idle<Drone>()))),
                                                          new DamageBuff<Drone>(new Move<Drone>(new SearchAlly<Drone>(new Idle<Drone>()))),
                                                          new SpeedDebuff<Drone>(new Move<Drone>(new SearchEnemy<Drone>(new Idle<Drone>())))};

        private RepairDroneState currentState;
        public RepairDroneState CurrentState
        { 
            get => currentState; 
            set
            {
                currentState = value;
                state = repairDroneState[(int)value];
            }
        }

        public override void InitState()
        {
            CurrentState = RepairDroneState.Repair;
        }

        public override void Spawn(Point spawnPoint)
        {
            base.Spawn(spawnPoint);
            CurrentState = D.SelfPlayer.RepairDroneState;
        }

        public override void UseIndividuality()
        {
        }
    }
}
