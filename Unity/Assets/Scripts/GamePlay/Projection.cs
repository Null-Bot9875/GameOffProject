using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameEvent;
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
        private Vector2 lineEndPos;
        [SerializeField] private GameObject endPosGo;
        private bool checkWall;
        public bool lineTouchPlayer;

        private readonly List<KeyValuePair<Transform, Transform>> _spawnedObjects =
            new List<KeyValuePair<Transform, Transform>>();
        private readonly List<GameObject> ghostObj = new List<GameObject>();

        // [SerializeField, Header("场景更新周期")] private float fixUpdateTime = 1f;
        // private float nowTime;

        private void Start()
        {
            TypeEventSystem.Global.Register<GameCloseEndPointEvt>(CloseEndPoint)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            TypeEventSystem.Global.Register<GameOpenEndPointEvt>(OpenEndPoint)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            _line.positionCount = _maxFrameIterations;
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
            checkWall = false;
            endPosGo.GetComponent<SpriteRenderer>().enabled = false;
            lineTouchPlayer = false;
        }

        private void Update()
        {
            foreach (var item in _spawnedObjects) {
                item.Value.position = item.Key.position;
                item.Value.rotation = item.Key.rotation;
            }

            if (checkWall)
            {
                endPosGo.GetComponent<SpriteRenderer>().enabled = true;
                endPosGo.transform.position = lineEndPos;
            }

            if (lineTouchPlayer)
            {
                endPosGo.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                endPosGo.GetComponent<SpriteRenderer>().enabled = true;
            }

            // if (Time.time > fixUpdateTime + nowTime)
            // {
            //     nowTime = Time.time;
            //     UpdateSceneTransform();
            // }

        }

        private void OpenEndPoint(GameOpenEndPointEvt gameOpenEndPointEvt)
        {
            lineTouchPlayer = false;
        }

        public void CloseEndPoint(GameCloseEndPointEvt gameCloseEndPointEvt)
        {
            lineTouchPlayer = true;
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
                    _spawnedObjects.Add(new KeyValuePair<Transform, Transform>(item.transform,ghostObj.transform));
                    
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

                if (i>1)
                {
                    if (_line.GetPosition(i) == _line.GetPosition(i-1))
                    {
                        lineEndPos = _line.GetPosition(i);
                        checkWall = true;
                    }
                }
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
