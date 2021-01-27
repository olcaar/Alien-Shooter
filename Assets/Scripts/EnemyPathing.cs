using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    // config params
    [SerializeField] WaveConfig waveConfig;
    List<Transform> wayPoints;

    // state variables
    int waypointIndex = 0;

    void Start()
    {
        wayPoints = waveConfig.GetWaypoints();
        transform.position = wayPoints[waypointIndex].transform.position;
    }

    void Update()
    {
        MoveInPath();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void MoveInPath()
    {
        if (waypointIndex <= wayPoints.Count - 1)
        {
            var targetPosition = wayPoints[waypointIndex].transform.position;
            var movmentThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movmentThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
