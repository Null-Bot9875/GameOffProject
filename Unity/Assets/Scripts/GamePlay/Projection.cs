using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Projection : MonoBehaviour
    {
        [SerializeField, Header("障碍物")] private Transform _objParent;
        [SerializeField] private LineRenderer _line;
        [SerializeField] private int _maxFrameIterations;
        [SerializeField] private GameObject endPosGo;
        [SerializeField] private float _radius;
        [SerializeField] private Vector2 _sphereCenter;


        private Scene _simulationScene;
        private PhysicsScene2D _physicsScene;

        private readonly List<KeyValuePair<Transform, Transform>> _spawnedObjects =
            new List<KeyValuePair<Transform, Transform>>();

        private readonly List<GameObject> _ghostList = new List<GameObject>();

        // private void OnDrawGizmos()
        // {
        //     GameDataCache.Instance.Player = GameObject.FindObjectOfType<PlayerController>();
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawWireSphere(GameDataCache.Instance.Player.transform.position+(Vector3)_sphereCenter,_radius);
        // }


        // [SerializeField, Header("场景更新周期")] private float fixUpdateTime = 1f;
        // private float nowTime;

        private void Start()
        {
            _line.positionCount = _maxFrameIterations;
            InitPhysicsScene();
        }

        public void Enable()
        {
            GameDataCache.Instance.Player = GameObject.FindObjectOfType<PlayerController>();
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
            foreach (var item in _spawnedObjects)
            {
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

                    _spawnedObjects.Add(new KeyValuePair<Transform, Transform>(item.transform, ghostObj.transform));
                }
            }
        }

        void DeleteSceneTransform()
        {
            foreach (var item in _ghostList)
            {
                Destroy(item);
            }

            _ghostList.Clear();
            _spawnedObjects.Clear();
        }

        public void SimulateTrajectory(BulletCtr bulletCtr, Vector2 direction)
        {
            var ghostObj = CreatGhostObj(bulletCtr.gameObject);
            ghostObj.GetComponent<BulletCtr>().SetFire(direction);

            for (int i = 0; i < _line.positionCount; i++)
            {
                _physicsScene.Simulate(Time.fixedDeltaTime);
                _line.SetPosition(i, ghostObj.transform.position);

                if (i == _line.positionCount - 1)
                {
                    var lastPosition = _line.GetPosition(i);
                    endPosGo.transform.position = lastPosition;
                    var enable = _line.GetPosition(i) == _line.GetPosition(i - 1);
                    var playerPosition = GameDataCache.Instance.Player.transform.position + (Vector3)_sphereCenter;
                    enable &= Vector2.Distance(lastPosition, playerPosition) > _radius;
                    endPosGo.GetComponent<SpriteRenderer>().enabled = enable;
                }
            }

            Destroy(ghostObj.gameObject);
        }

        private GameObject CreatGhostObj(GameObject go)
        {
            var ghostObj = Instantiate(go.gameObject, go.transform.position, go.transform.rotation);
            if (!ghostObj.CompareTag("Bullet"))
            {
                _ghostList.Add(ghostObj);
            }

            ghostObj.GetComponent<SpriteRenderer>().enabled = false;
            var shadow = ghostObj.GetComponent<ShadowCaster2D>();
            if (shadow != null)
            {
                shadow.enabled = false;
            }

            var light = ghostObj.GetComponentInChildren<Light2D>();
            if (light != null)
            {
                light.enabled = false;
            }

            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            return ghostObj;
        }
    }
}