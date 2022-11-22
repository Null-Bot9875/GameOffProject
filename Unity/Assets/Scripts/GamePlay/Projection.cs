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
            if (_simulationScene.isLoaded)
                return;

            _simulationScene = SceneManager.CreateScene("simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
            _physicsScene = _simulationScene.GetPhysicsScene2D();
        }

        public void Enable()
        {
            if (_line.gameObject.activeSelf)
                return;
            
            _line.gameObject.SetActive(true);
            _line.enabled = true;
            endPosGo.GetComponent<SpriteRenderer>().enabled = true;
            
            foreach (Transform item in _objParent)
            {
                if (item.CompareTag("Enemy"))
                    continue;
                var ghostObj = CreatGhostObj(item.gameObject);
                if (!ghostObj.isStatic)
                {
                    _spawnedObjects.Add(new KeyValuePair<Transform, Transform>(item, ghostObj.transform));
                }
            }
        }

        public void Disable()
        {
            if (!_line.gameObject.activeSelf)
                return;
            
            _line.gameObject.SetActive(false);
            _line.enabled = false;
            endPosGo.GetComponent<SpriteRenderer>().enabled = false;
            
            _spawnedObjects.Clear();
            foreach (var go in _simulationScene.GetRootGameObjects())
            {
                GameObject.Destroy(go);
            }
        }

        private void Update()
        {
            foreach (var item in _spawnedObjects)
            {
                item.Value.position = item.Key.position;
                item.Value.rotation = item.Key.rotation;
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
                    var distance = Vector2.Distance(lastPosition, playerPosition);
                    enable &= distance > _radius;
                    endPosGo.GetComponent<SpriteRenderer>().enabled = enable;
                }
            }
        }


        private GameObject CreatGhostObj(GameObject ghostGo)
        {
            var go = ghostGo.CompareTag("Player") ? CreatPlayerGo(ghostGo) : CopyTmpGo(ghostGo);
            SceneManager.MoveGameObjectToScene(go, _simulationScene);
            return go;
        }

        private static GameObject CopyTmpGo(GameObject ghostGo)
        {
            var go = Instantiate(ghostGo);
            var sprite = go.GetComponent<SpriteRenderer>();
            if (sprite)
            {
                sprite.enabled = false;
            }

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

        private static GameObject CreatPlayerGo(GameObject ghostGo)
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