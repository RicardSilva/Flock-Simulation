using UnityEngine;
using System.Collections;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicArrive : DynamicVelocityMatch
    {

        public override string Name
        {
            get { return "DynamicArrive"; }
        }

        public KinematicData ArriveTarget { get; set; }
        public float MaxSpeed { get; set; }
        public float StopRadius { get; set; }
        public float SlowRadius { get; set; }

        public DynamicArrive()
        {
            this.MaxSpeed = 10.0f;
            this.StopRadius = 2.5f;
            this.SlowRadius = 4.0f;
            this.Target = new KinematicData();
        }

        public override MovementOutput GetMovement()
        {

            var direction = this.ArriveTarget.position - this.Character.position;
            var distance = direction.magnitude;
            float targetSpeed = 0.0f;

            if (distance < this.StopRadius)
                targetSpeed = 0;
            if (distance > this.SlowRadius)
                targetSpeed = this.MaxSpeed;
            else
                targetSpeed = this.MaxSpeed * (distance / this.SlowRadius);
            
            this.Target.velocity = direction.normalized * targetSpeed;

            return base.GetMovement();
        }
    }
}
