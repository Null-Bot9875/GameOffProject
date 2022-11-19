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


        private void OnDrawGizmos()
        {
            GameDataCache.Instance.Player = GameObject.FindObjectOfType<PlayerController>();
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(GameDataCache.Instance.Player.transform.position + (Vector3)_sphereCenter, _radius);
        }

        private void Start()
        {
            _line.positionCount = _maxFrameIterations;
            InitPhysicsScene();
        }

        public void Enable()
        {
            _line.gameObject.SetActive(true);
            GameDataCache.Instance.Player = GameObject.FindObjectOfType<PlayerController>();
            InitSceneTransform();
            _line.enabled = true;
            endPosGo.GetComponent<SpriteRenderer>().enabled = true;
        }

        public void Disable()
        {
            _line.gameObject.SetActive(false);
            foreach (var go in _simulationScene.GetRootGameObjects())
            {
                GameObject.Destroy(go);
            }

            _spawnedObjects.Clear();
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
        }

        private void InitPhysicsScene()
        {
            if (_simulationScene.isLoaded)
                return;

            _simulationScene =
                SceneManager.CreateScene("simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
            _physicsScene = _simulationScene.GetPhysicsScene2D();
        }

        void InitSceneTransform()
        {
            foreach (Transform item in _objParent)
            {
                var ghostObj = CreatGhostObj(item.gameObject);
                if (!ghostObj.isStatic)
                {
                    _spawnedObjects.Add(new KeyValuePair<Transform, Transform>(item, ghostObj.transform));
                }
            }
        }

        public void SimulateLinePosition(BulletCtr bulletCtr, Vector2 direction)
        {
            SceneManager.MoveGameObjectToScene(bulletCtr.gameObject, _simulationScene);
            bulletCtr.SetFire(direction);
            
            for (int i = 0; i < _line.positionCount; i++)
            {
                _physicsScene.Simulate(Time.fixedDeltaTime);
                _line.SetPosition(i, bulletCtr.transform.position);

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
        }


        private GameObject CreatGhostObj(GameObject ghostGo)
        {
            var go = ghostGo.CompareTag("Player") ? CreatTmpGo(ghostGo) : CopyTmpGo(ghostGo);
            SceneManager.MoveGameObjectToScene(go, _simulationScene);
            return go;
        }

        private static GameObject CopyTmpGo(GameObject ghostGo)
        {
            var go = Instantiate(ghostGo);
            go.GetComponent<SpriteRenderer>().enabled = false;
            var shadow = go.GetComponent<ShadowCaster2D>();
            if (shadow != null)
            {
                shadow.enabled = false;
            }

            var light = go.GetComponentInChildren<Light2D>();
            if (light != null)
            {
                light.enabled = false;
            }

            return go;
        }

        private static GameObject CreatTmpGo(GameObject ghostGo)
        {
            var tmpGo = new GameObject();
            tmpGo.tag = ghostGo.tag;
            tmpGo.layer = ghostGo.layer;
            tmpGo.name = ghostGo.name;
            tmpGo.transform.position = ghostGo.transform.position;
            tmpGo.transform.rotation = ghostGo.transform.rotation;
            tmpGo.transform.localScale = ghostGo.transform.localScale;

            var collider = ghostGo.GetComponent<Collider2D>();
            switch (collider)
            {
                case BoxCollider2D tmpBox:
                {
                    var box = tmpGo.AddComponent<BoxCollider2D>();
                    box.size = tmpBox.size;
                    box.offset = tmpBox.offset;
                    break;
                }
                case CircleCollider2D tmpCircle:
                {
                    var circle = tmpGo.AddComponent<CircleCollider2D>();
                    circle.radius = tmpCircle.radius;
                    circle.offset = tmpCircle.offset;
                    break;
                }
            }

            var rb = ghostGo.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                var tmpRb = tmpGo.AddComponent<Rigidbody2D>();
                tmpRb.bodyType = RigidbodyType2D.Static;
                tmpRb.sharedMaterial = rb.sharedMaterial;
                tmpRb.gravityScale = rb.gravityScale;
            }

            return tmpGo;
        }
    }
}