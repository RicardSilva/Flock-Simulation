using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.IAJ.Unity.Util;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class FlockCohesion : DynamicArrive
    {
        public override string Name
        {
            get { return "Cohesion"; }
        }

        public List<KinematicData> Flock { get; set; }
        public float Radius { get; set; }
        public float FanAngle { get; set; }


        public FlockCohesion()
        {
            //TODO: TWEAK VARIABLES
            this.Radius = 30.0f;
            this.FanAngle = MathConstants.MATH_PI;
            this.ArriveTarget = new KinematicData();
        }

        public override MovementOutput GetMovement() {
            var massCenter = new Vector3();
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
                            massCenter += boid.position;
                            closeBoids++;
                        }
                    }
                }
            }
            if (closeBoids == 0)
                return null;
            massCenter /= closeBoids;

            this.ArriveTarget.position = massCenter;
            return base.GetMovement();
        }

    }
}