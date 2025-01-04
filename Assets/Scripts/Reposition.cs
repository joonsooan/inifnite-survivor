using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private Collider2D coll;
    private int mapLength;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        mapLength = 32;
    }

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

        // �÷��̾��� �̵� ����
        Vector3 playerDir = GameManager.Instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1; // ���� : ������
        float dirY = playerDir.y < 0 ? -1 : 1; // �Ʒ� : ��

        switch (transform.tag) // ������Ʈ�� ���� ���
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * (mapLength * 2));
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * (mapLength * 2));
                }
                break;

            case "Enemy":
                if (coll.enabled)
                {   // �÷��̾� �̵����� ���� ���� �����
                    transform.Translate(playerDir * mapLength
                        + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }
}