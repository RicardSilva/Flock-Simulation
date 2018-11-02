using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class FlockSeparation : DynamicMovement{

        public override string Name
        {
            get { return "Separation"; }
        }

        public override KinematicData Target { get; set; }
        public List<KinematicData> Flock { get; set; }
        public float SeparationFactor { get; set; }
        public float Radius { get; set; }

        public FlockSeparation()
        {
            //TODO: TWEAK VARIABLES
            this.SeparationFactor = 20.0f;
            this.Radius = 30.0f;
        }

        public override MovementOutput GetMovement() {
            var output = new MovementOutput();
            KinematicData character = this.Character;
            float sqrRadius = this.Radius * this.Radius;

            foreach (var boid in this.Flock) {
                if (boid != character) {
                    var direction = character.position - boid.position;
                    if (direction.sqrMagnitude < sqrRadius) {
                        var separationStrength = Math.Min(this.SeparationFactor / (direction.sqrMagnitude), this.MaxAcceleration);
                        output.linear += direction * separationStrength;
                        
                    }
                }
            }
            if (output.linear.sqrMagnitude > this.MaxAcceleration * this.MaxAcceleration)
                output.linear = output.linear.normalized * this.MaxAcceleration;

            return output;
        }


    }
}
