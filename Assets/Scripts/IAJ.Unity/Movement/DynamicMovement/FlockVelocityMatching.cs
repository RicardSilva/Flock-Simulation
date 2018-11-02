using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.IAJ.Unity.Util;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class FlockVelocityMatching : DynamicVelocityMatch
    {
     
        public List<KinematicData> Flock { get; set; }
        public float Radius { get; set; }
        public float FanAngle { get; set; }

        public FlockVelocityMatching()
        {
            //TODO: TWEAK VARIABLES
            this.Radius = 30.0f;
            this.FanAngle = MathConstants.MATH_PI;
            this.Target = new KinematicData();

        }

        public override MovementOutput GetMovement() {
            var averageVelocity = new Vector3();
            var direction = new Vector3();
            var closeBoids = 0;

            KinematicData character = this.Character;
            float sqrRadius = this.Radius * this.Radius;

            foreach (var boid in this.Flock) {
                if (character != boid) {
                    direction = boid.position - character.position;

                    if (direction.sqrMagnitude <= sqrRadius) {
                        var angle = MathHelper.ConvertVectorToOrientation(direction);
                        var angleDifference = MathHelper.ShortestAngleDifference(character.orientation, angle);

                        if (Math.Abs(angleDifference) <= this.FanAngle) {
                            averageVelocity += boid.velocity;
                            closeBoids++;
                        }
                    }
                }
            }

            if (closeBoids == 0)
                return null;
            averageVelocity /= closeBoids;
            
            this.Target.velocity = averageVelocity.normalized * MaxAcceleration;

            return base.GetMovement();
        }



    }
}
