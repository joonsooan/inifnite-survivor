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

    // 플레이어 위치에 따라 타일맵을 이동시키는 코드
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        // 플레이어와 오브젝트 사이의 거리 계산
        Vector3 playerPosition = GameManager.Instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPosition.x - myPos.x);
        float diffY = Mathf.Abs(playerPosition.y - myPos.y);

        // 플레이어의 이동 방향
        Vector3 playerDir = GameManager.Instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1; // 왼쪽 : 오른쪽
        float dirY = playerDir.y < 0 ? -1 : 1; // 아래 : 위

        switch (transform.tag) // 오브젝트에 따른 기능
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
                {   // 플레이어 이동방향 맞은 편에서 재등장
                    transform.Translate(playerDir * mapLength
                        + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                }
                break;
        }
    }
}