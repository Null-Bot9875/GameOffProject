using UnityEngine;

namespace Game
{
    public class INeedCreatWall : MonoBehaviour
    {
        public bool 是否是固定外墙;
        public PhysicsMaterial2D P2D;

        private void Awake()
        {
            if (gameObject.GetComponent<Rigidbody2D>() != null)
            {
                return;
            }

            if (是否是固定外墙)
            {
                gameObject.AddComponent<BoxCollider2D>();
                gameObject.AddComponent<WallCtr>();
                gameObject.layer = LayerMask.NameToLayer("Wall");
                gameObject.isStatic = true;
            }
            else
            {
                gameObject.AddComponent<BoxCollider2D>();
                var rb = gameObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
                rb.sharedMaterial = P2D;
                gameObject.isStatic = true;
            }

            Destroy(this);
        }
    }
}