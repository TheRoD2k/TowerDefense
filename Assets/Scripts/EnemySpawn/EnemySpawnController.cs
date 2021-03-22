using Assets;
using Enemy;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace EnemySpawn
{
    public class EnemySpawnController : IController
    {

        public SpawnWavesAsset m_SpawnWaves;
        private Grid m_Grid;

        private float m_SpawnStartTime;
        private float m_PassedTimeAtPreviousFrame = -1f;

        public EnemySpawnController(SpawnWavesAsset mSpawnWaves, Grid mGrid)
        {
            m_SpawnWaves = mSpawnWaves;
            m_Grid = mGrid;
        }

        public void OnStart()
        {
            m_SpawnStartTime = Time.time;
        }

        public void OnStop()
        {

        }

        public void Tick()
        {
            float passedTime = Time.time - m_SpawnStartTime;
            float timeToSpawn = 0f;
            
            foreach (SpawnWave wave in m_SpawnWaves.SpawnWaves)
            {
                timeToSpawn += wave.TimeBeforeStartWave;

                for (int i = 0; i < wave.Count; i++)
                {
                    if (passedTime >= timeToSpawn && m_PassedTimeAtPreviousFrame < timeToSpawn)
                    {
                        // SpawnEnemy
                        SpawnEnemy(wave.EnemyAsset);
                    }

                    if (i < wave.Count - 1)
                    {
                        timeToSpawn += wave.TimeBetweenSpawns;
                    }
                }
            }

            m_PassedTimeAtPreviousFrame = passedTime;
        }

        private void SpawnEnemy(EnemyAsset asset)
        {
            EnemyView view = Object.Instantiate(asset.ViewPrefab);
            Vector3 startNodePosition = m_Grid.GetStartNode().Position;
            Vector3 viewSpawnPosition = new Vector3(startNodePosition.x, view.transform.position.y, startNodePosition.z);
            view.transform.position = viewSpawnPosition;
            EnemyData data = new EnemyData(asset);
            
            data.AttachView(view);
            view.CreateMovementAgent(m_Grid);
            
            Game.Player.EnemySpawned(data);
        }
    }
}