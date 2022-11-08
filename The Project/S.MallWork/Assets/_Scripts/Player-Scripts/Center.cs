using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[ExecuteInEditMode]
public class Center : MonoBehaviour
{
       [SerializeField] public Vector3 massOffset = Vector3.zero;
       [SerializeField] public bool _showing = true;
        private Vector3   centerOfMass, prevOffset = Vector3.zero;
        private Rigidbody rb;


        private void Awake() => rb = GetComponent<Rigidbody>();
        private void Start() => centerOfMass = rb.centerOfMass;


        private void Update()
        {
            if (massOffset != prevOffset) rb.centerOfMass = centerOfMass + massOffset;
            prevOffset = massOffset;
        }


        private void OnDrawGizmos()
        {
            if (!_showing || rb == null) return;
            var radius = 0.1f;
            try
            {
                radius = GetComponent<MeshFilter>().sharedMesh.bounds.size.z / 5f;
            }
            catch
            {
            }

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(rb.transform.TransformPoint(rb.centerOfMass), radius);
        }
}

