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


        private void Start()
        {
            CreatPhysicsScene();
        }

        private void Update()
        {
            foreach (var item in _spawnedObjects) {
                item.Value.position = item.Key.position;
                item.Value.rotation = item.Key.rotation;
            }
        }

        void CreatPhysicsScene()
        {
            _simulationScene =
                SceneManager.CreateScene("simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
            _physicsScene = _simulationScene.GetPhysicsScene2D();

            foreach (Transform VARIABLE in _objParent)
            {
                var ghostObj = CreatGhostObj(VARIABLE.gameObject, VARIABLE.position, VARIABLE.rotation);
                if (!ghostObj.isStatic)
                {
                    _spawnedObjects.Add(VARIABLE.transform,ghostObj.transform);
                }
            }

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
            return ghostObj;
        }
    }
}
