using UnityEngine;

namespace com.MLR.Wesp
{
    public class KeyboardPlayerController : PlayerController
    {
        protected override void UpdatePlayer()
        {
            //PC
            if (Input.GetKeyDown(KeyCode.W))
            {
                GameManager.Instance.levelManager.RemoveTile(this.x, this.y);

                this.y--;
                this.transform.Translate(new Vector3(0, GameManager.Instance.levelManager.ySize, 0));

                GameManager.Instance.OnPlayerMove(this);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                GameManager.Instance.levelManager.RemoveTile(this.x, this.y);

                this.x--;
                this.transform.Translate(new Vector3(-GameManager.Instance.levelManager.xSize, 0, 0));

                GameManager.Instance.OnPlayerMove(this);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                GameManager.Instance.levelManager.RemoveTile(this.x, this.y);

                this.y++;
                this.transform.Translate(new Vector3(0, -GameManager.Instance.levelManager.ySize, 0));

                GameManager.Instance.OnPlayerMove(this);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                GameManager.Instance.levelManager.RemoveTile(this.x, this.y);

                this.x++;
                this.transform.Translate(new Vector3(GameManager.Instance.levelManager.xSize, 0, 0));

                GameManager.Instance.OnPlayerMove(this);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GameManager.Instance.levelManager.RemoveTile(this.x, this.y);

                this.y -= 2;
                this.transform.Translate(new Vector3(0, GameManager.Instance.levelManager.ySize * 2, 0));

                GameManager.Instance.OnPlayerMove(this);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                GameManager.Instance.levelManager.RemoveTile(this.x, this.y);

                this.x -= 2;
                this.transform.Translate(new Vector3(-GameManager.Instance.levelManager.xSize * 2, 0, 0));

                GameManager.Instance.OnPlayerMove(this);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                GameManager.Instance.levelManager.RemoveTile(this.x, this.y);

                this.y += 2;
                this.transform.Translate(new Vector3(0, -GameManager.Instance.levelManager.ySize * 2, 0));

                GameManager.Instance.OnPlayerMove(this);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                GameManager.Instance.levelManager.RemoveTile(this.x, this.y);

                this.x += 2;
                this.transform.Translate(new Vector3(GameManager.Instance.levelManager.xSize * 2, 0, 0));

                GameManager.Instance.OnPlayerMove(this);
            }
        }
    }
}