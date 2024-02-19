using System.Collections;
using UnityEngine;

public class ClockHandler : MonoBehaviour
{
    [SerializeField] Transform hourStick;
    [SerializeField] Transform minuteStick;
    [SerializeField] Transform secondStick;

    private void Start()
    {
        StartCoroutine(Clock());
    }

    IEnumerator Clock()
    {
        while (true)
        {
            System.DateTime currentTime = System.DateTime.Now;

            int hour = currentTime.Hour;
            int minute = currentTime.Minute;
            int second = currentTime.Second;

            // Calculate rotation angles
            float hourAngle = (hour % 12) * 30f + minute * 0.5f; // 30 degrees per hour, 0.5 degrees per minute
            float minuteAngle = minute * 6f; // 6 degrees per minute
            float secondAngle = second * 6f; // 6 degrees per second

            // Apply rotations
            hourStick.localRotation = Quaternion.Euler(hourAngle, 0, 0);
            minuteStick.localRotation = Quaternion.Euler(minuteAngle, 0, 0);
            secondStick.localRotation = Quaternion.Euler(secondAngle, 0, 0);

            yield return new WaitForSeconds(1);
        }
    }
}
