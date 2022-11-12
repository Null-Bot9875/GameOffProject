using System.Collections.Generic;
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
        private readonly List<KeyValuePair<Transform, Transform>> _spawnedObjects =
            new List<KeyValuePair<Transform, Transform>>();
        private readonly List<GameObject> ghostObj = new List<GameObject>();

        // [SerializeField, Header("场景更新周期")] private float fixUpdateTime = 1f;
        // private float nowTime;

        private void Start()
        {
            _line.positionCount = _maxFrameIterations;
            InitPhysicsScene();
        }

        public void Enable()
        {
            UpdateSceneTransform();
            _line.enabled = true;
            endPosGo.GetComponent<SpriteRenderer>().enabled = true;
        }

        public void Disable()
        {
            DeleteSceneTransform();
            _line.enabled = false;
            endPosGo.GetComponent<SpriteRenderer>().enabled = false;
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
                var ghostObj = CreatGhostObj(item.gameObject);
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

        public void SimulateTrajectory(BulletCtr bulletCtr,Vector2 direction)
        {
            var ghostObj = CreatGhostObj(bulletCtr.gameObject);
            
            if (ghostObj.GetComponent<BulletCtr>().isBack)
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
                        endPosGo.transform.position = lineEndPos;
                    }
                }
            }
            Destroy(ghostObj.gameObject);
        }

        private GameObject CreatGhostObj(GameObject go)
        {
            var ghostObj = Instantiate(go.gameObject,go.transform.position, go.transform.rotation);
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
