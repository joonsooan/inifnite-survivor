using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    // �÷��̾� ��ġ�� ���� Ÿ�ϸ��� �̵���Ű�� �ڵ�
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        // �÷��̾�� ������Ʈ ������ �Ÿ� ���
        Vector3 playerPosition = GameManager.Instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPosition.x - myPos.x);
        float diffY = Mathf.Abs(playerPosition.y - myPos.y);

        // ������Ʈ ���� �÷��̾��� ��ġ
        Vector3 playerDir = GameManager.Instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1; // ���� ������
        float dirY = playerDir.y < 0 ? -1 : 1; // �Ʒ� ��

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":
                break;
        }
    }
}