using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Projection : MonoBehaviour
    {
        private Scene _simulationScene;
        private PhysicsScene2D _physicsScene;
        [SerializeField, Header("障碍物")] private Transform _objParent;
        [SerializeField] private LineRenderer _line;
        [SerializeField] private int _maxFrameIterations;
        private readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();
        private readonly List<GameObject> ghostObj = new List<GameObject>();

        // [SerializeField, Header("场景更新周期")] private float fixUpdateTime = 1f;
        // private float nowTime;

        private void Start()
        {
            InitPhysicsScene();
        }

        public void Enable()
        {
            UpdateSceneTransform();
            _line.enabled = true;
        }

        public void Disable()
        {
            DeleteSceneTransform();
            _line.enabled = false;
        }

        private void Update()
        {
            foreach (var item in _spawnedObjects) {
                item.Value.position = item.Key.position;
                item.Value.rotation = item.Key.rotation;
            }

            // if (Time.time > fixUpdateTime + nowTime)
            // {
            //     nowTime = Time.time;
            //     UpdateSceneTransform();
            // }
            
        }

        public void InitPhysicsScene()
        {
            if (_simulationScene.isLoaded)
            {
                return;
            }
            _simulationScene =
                SceneManager.CreateScene("simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
            _physicsScene = _simulationScene.GetPhysicsScene2D();
        }

        void UpdateSceneTransform()
        {
            foreach (Transform item in _objParent)
            {
                var ghostObj = CreatGhostObj(item.gameObject, item.position, item.rotation);
                if (!ghostObj.isStatic)
                {
                    if (ghostObj.CompareTag("Player"))
                    {
                        ghostObj.GetComponent<PlayerController>().enabled = false;
                        ghostObj.GetComponent<Collider2D>().isTrigger = true;
                        for (int i = 0; i < ghostObj.transform.childCount; i++)
                        {
                            ghostObj.transform.GetChild(i).gameObject.SetActive(false);
                            
                        }
                        
                    }
                    _spawnedObjects.Add(item.transform,ghostObj.transform);
                    
                }
            }
        }

        void DeleteSceneTransform()
        {
            foreach (var item in ghostObj)
            {
                Destroy(item);
            }
            ghostObj.Clear();
            _spawnedObjects.Clear();
        }

        public void SimulateTrajectory(BulletCtr bulletCtr,Vector2 StartShootPos,Quaternion quaternion,Vector2 direction)
        {
            var ghostObj = CreatGhostObj(bulletCtr.gameObject, StartShootPos, quaternion);
            _line.positionCount = _maxFrameIterations;
            if (ghostObj.GetComponent<BulletCtr>().isback)
            {
                ghostObj.GetComponent<BulletCtr>().SetFire(direction, true,true);
            }
            else
            {
                ghostObj.GetComponent<BulletCtr>().SetFire(direction, true);
            }
            for (int i = 0; i < _line.positionCount; i++)
            {
                _physicsScene.Simulate(Time.fixedDeltaTime);
                _line.SetPosition(i,ghostObj.transform.position);
            }
            Destroy(ghostObj.gameObject);
        }

        private GameObject CreatGhostObj(GameObject go, Vector2 pos, Quaternion quaternion)
        {
            var ghostObj = Instantiate(go.gameObject, pos, quaternion);
            ghostObj.GetComponent<SpriteRenderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj,_simulationScene);
            if (go.GetComponent<BulletCtr>() == null)
            {
                this.ghostObj.Add(ghostObj);
            }
            return ghostObj;
        }
        
        
    }
}
