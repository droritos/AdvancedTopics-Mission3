//using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

//public class BossTwoMovment : MonoBehaviour
//{
//    BossLevelTwo bossLevelTwo;
//    BossManager bossManager;
//    void Start()
//    {
//        bossLevelTwo = GetComponent<BossLevelTwo>();
//        bossManager = GetComponent<BossManager>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        HandleMovement();
//    }
//    private void HandleMovement()
//    {
//        float distanceToPlayer = Vector2.Distance(transform.position, bossLevelTwo.player.position);

//        if (distanceToPlayer <= bossLevelTwo.maxLineOfSite)
//        {
//            // Zigzag movement calculation
//            bossLevelTwo.zigzagTimer += Time.deltaTime * bossLevelTwo.zigzagFrequency;
//            float xOffset = Mathf.Sin(bossLevelTwo.zigzagTimer) * bossLevelTwo.zigzagAmplitude;

//            // Move towards the player on the y-axis and zigzag on the x-axis
//            Vector3 targetPosition = new Vector3(transform.position.x + xOffset, bossLevelTwo.player.position.y, transform.position.z);
//            Vector3 direction = (targetPosition - transform.position).normalized;

//            bossManager._velocity.MovePosition(transform.position + direction * bossLevelTwo.speed * Time.deltaTime);
//        }
//    }

//}
