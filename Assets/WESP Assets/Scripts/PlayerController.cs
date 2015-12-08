using UnityEngine;

namespace com.MLR.Wesp
{
    public abstract class PlayerController : MonoBehaviour
    {
        public int x;
        public int y;

        // Use this for initialization
        void Start()
        {
            this.x = Mathf.RoundToInt(this.transform.position.x / GameManager.Instance.levelManager.xSize);
            this.y = GameManager.Instance.levelManager.CountRows() - Mathf.RoundToInt(this.transform.position.y / GameManager.Instance.levelManager.ySize) - 1;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.state == GameManager.GameState.Playing && Time.timeScale == 1)
            {
                this.UpdatePlayer();
            }
        }

        protected abstract void UpdatePlayer();
    }
}