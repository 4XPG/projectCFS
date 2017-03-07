using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

    [RequireComponent(typeof (AeroplanePhysics))]
    public class AeroplaneUserControlSim : MonoBehaviour
    {

        // reference to the aeroplane that we're controlling
        private AeroplanePhysics m_Aeroplane;
        private float m_Throttle;
        private bool m_AirBrakes;
        private float m_Yaw;


        private void Awake()
        {
            // Set up the reference to the aeroplane controller.
            m_Aeroplane = GetComponent<AeroplanePhysics>();
        }


        private void FixedUpdate()
        {
            // Read input for the pitch, yaw, roll and throttle of the aeroplane.
            float roll = CrossPlatformInputManager.GetAxis("Horizontal");
            float pitch = CrossPlatformInputManager.GetAxis("Vertical");
            m_AirBrakes = CrossPlatformInputManager.GetButton("Fire3");
            m_Yaw = CrossPlatformInputManager.GetAxis("Yaw");
            m_Throttle = CrossPlatformInputManager.GetAxis("Throttle");

            // Pass the input to the aeroplane
            m_Aeroplane.Move(roll, pitch, m_Yaw, m_Throttle, m_AirBrakes);
        }

    }

