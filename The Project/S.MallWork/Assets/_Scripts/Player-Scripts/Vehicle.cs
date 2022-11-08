using UnityEngine;

    public class Vehicle : MonoBehaviour
    {
        public bool  vehicleIsActive = true;
        public float BrakeSpeed, Speed;
       [Range(0, 360)]  public float maxAngle = 35, minAngle = 20;
       public AnimationCurve torqueSpeedCurve;

       [HideInInspector]
       public float velocity;
        protected float smoothX, velocityX;
        public void Active(bool state) => vehicleIsActive = state;
    }
