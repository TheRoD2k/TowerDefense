﻿using System.Collections.Generic;
using Enemy;
using Field;
using Turret;
using Turret.Weapon;
using TurretSpawn;
using UnityEngine;
using Grid = Field.Grid;

namespace Runtime
{
    public class Player
    {
        private List<EnemyData> m_EnemyDatas = new List<EnemyData>();

        public IReadOnlyList<EnemyData> EnemyDatas => m_EnemyDatas;

        private List<TurretData> m_TurretDatas = new List<TurretData>();

        public List<TurretData> TurretDatas => m_TurretDatas;

        public readonly GridHolder GridHolder;
        public readonly Grid Grid;
        public readonly TurretMarket TurretMarket;
        //public readonly EnemySearch EnemySearch;

        public Player()
        {
            GridHolder = Object.FindObjectOfType<GridHolder>();
            GridHolder.CreateGrid();
            Grid = GridHolder.Grid;

            TurretMarket = new TurretMarket(Game.CurrentLevel.TurretMarketAsset);

            //EnemySearch = new EnemySearch(m_EnemyDatas);
        }
        
        public void EnemySpawned(EnemyData data)
        {
            m_EnemyDatas.Add(data);
        }

        public void EnemyDied(EnemyData data)
        {
            m_EnemyDatas.Remove(data);
        }

        public void TurretSpawned(TurretData data)
        {
            m_TurretDatas.Add(data);
        }
    }
}