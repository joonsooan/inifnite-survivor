using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
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

        // 오브젝트 기준 플레이어의 위치
        Vector3 playerDir = GameManager.Instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1; // 왼쪽 오른쪽
        float dirY = playerDir.y < 0 ? -1 : 1; // 아래 위

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