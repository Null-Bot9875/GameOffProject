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
        

        private void Start()
        {
            CreatPhysicsScene();
        }

        void CreatPhysicsScene()
        {
            _simulationScene =
                SceneManager.CreateScene("simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
            _physicsScene = _simulationScene.GetPhysicsScene2D();

            foreach (Transform VARIABLE in _objParent)
            {
                CreatGhostObj(VARIABLE.gameObject, VARIABLE.position, VARIABLE.rotation);
            }

        }

        public void SimulateTrajectory(BulletCtr bulletCtr,Vector2 muzzlePos,Quaternion quaternion,Vector2 direction)
        {
            var ghostObj = CreatGhostObj(bulletCtr.gameObject, muzzlePos, quaternion);
            _line.positionCount = _maxFrameIterations;
            ghostObj.GetComponent<BulletCtr>().SetFire(direction);
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
