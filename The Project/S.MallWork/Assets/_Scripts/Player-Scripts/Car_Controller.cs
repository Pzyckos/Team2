using System;
using System.Collections.Generic;
using UnityEngine;
using static InputManager;

[RequireComponent(typeof(Rigidbody))]
public class Car_Controller : Vehicle
{
    [SerializeField] private List<_WheelWheelCollider> listWheels;

    private Rigidbody _rigidbody;
    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    public void FixedUpdate()
    {
        if (!vehicleIsActive) return;
        var xAxis = Input.GetAxis("Horizontal");
        var yAxis = Input.GetAxis("Vertical");

        float absYAxis = yAxis < 0 ? -yAxis : yAxis;

        velocity = transform.InverseTransformDirection(_rigidbody.velocity).z;
        smoothX = Mathf.SmoothDamp(smoothX, xAxis, ref velocityX, 0.12f);

        float motorTorque = torqueSpeedCurve.Evaluate(Mathf.Abs(velocity * 0.02f)) * Speed;

        foreach (var w in listWheels)
        {
            w.wheelCollider.brakeTorque = 0f;
            w.wheelCollider.motorTorque = 0f;

            if (Input.GetKey(KeyCode.Space)) w.wheelCollider.brakeTorque = BrakeSpeed;

            if (velocity < -0.4f && yAxis > 0.1f || velocity > 0.4f && yAxis < -0.1f)
                w.wheelCollider.brakeTorque = BrakeSpeed * Mathf.Abs(yAxis);

            if (w.wheelCollider.brakeTorque < 0.01f)
                if (velocity >= -0.5f && yAxis > 0.1f || velocity <= 0.5f && yAxis < -0.1f)
                    w.wheelCollider.motorTorque = motorTorque * yAxis;


            if (w.steer)
                w.wheelCollider.steerAngle =
                    Mathf.Lerp(maxAngle, minAngle, Mathf.Abs(velocity) * 0.05f) * xAxis;

            if (w.wheelModel == null) continue;
            Vector3 position;
            Quaternion rotation;
            w.wheelCollider.GetWorldPose(out position, out rotation);
            w.wheelModel.SetPositionAndRotation(position, rotation);
        }
    }

    public void OnBrakeValueChanged(float a) => BrakeSpeed = a;
    public void OnMotorValueChanged(float v) => Speed = v;

}

[Serializable]
        public struct _WheelWheelCollider
        {
            public bool power;
            public bool steer;
            public Transform wheelModel;
            public WheelCollider wheelCollider;
        }

