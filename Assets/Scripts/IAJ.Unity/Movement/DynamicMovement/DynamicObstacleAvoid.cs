using UnityEngine;
using System.Collections;
using Assets.Scripts.IAJ.Unity.Util;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

    public class DynamicObstacleAvoid : DynamicSeek
    {

        public override string Name
        {
            get { return "ObstacleAvoid"; }
        }
        
        public float AvoidMargin { get; set; }
        public float MaxLookAhead { get; set; }
        public float WhiskersLookAhead { get; set; }
        public float BackWhiskersLookAhead { get; set; }

        public DynamicObstacleAvoid()
        {
            //TODO: TWEAK VARIABLES
            this.AvoidMargin = 5.0f;
            this.MaxLookAhead = 6.0f;
            this.WhiskersLookAhead = 4.0f;
            this.BackWhiskersLookAhead = 1.8f;

        }

        public override MovementOutput GetMovement()
        {
            RaycastHit hit;

            Debug.DrawRay(this.Character.position, this.Character.velocity.normalized * this.MaxLookAhead, MovementDebugColor);
            Debug.DrawRay(this.Character.position, MathHelper.Rotate2D(this.Character.velocity,  MathConstants.MATH_PI / 5).normalized * this.WhiskersLookAhead, MovementDebugColor);
            Debug.DrawRay(this.Character.position, MathHelper.Rotate2D(this.Character.velocity, -MathConstants.MATH_PI / 5).normalized * this.WhiskersLookAhead, MovementDebugColor);
            //Debug.DrawRay(this.Character.position, MathHelper.Rotate2D(this.Character.velocity,  MathConstants.MATH_PI_2).normalized * this.BackWhiskersLookAhead, MovementDebugColor);
            //Debug.DrawRay(this.Character.position, MathHelper.Rotate2D(this.Character.velocity, -MathConstants.MATH_PI_2).normalized * this.BackWhiskersLookAhead, MovementDebugColor);


            if ((Physics.Raycast(this.Character.position, this.Character.velocity, out hit, this.MaxLookAhead) == false)
                && (Physics.Raycast(this.Character.position, MathHelper.Rotate2D(this.Character.velocity, MathConstants.MATH_PI / 5), out hit, this.WhiskersLookAhead) == false)
                && (Physics.Raycast(this.Character.position, MathHelper.Rotate2D(this.Character.velocity, -MathConstants.MATH_PI / 5), out hit, this.WhiskersLookAhead) == false))
                //&&  (Physics.Raycast(this.Character.position, MathHelper.Rotate2D(this.Character.velocity,  MathConstants.MATH_PI_2), out hit, this.BackWhiskersLookAhead) == false)
                //&&  (Physics.Raycast(this.Character.position, MathHelper.Rotate2D(this.Character.velocity, -MathConstants.MATH_PI_2), out hit, this.BackWhiskersLookAhead) == false))
                return null;
            
            this.Target = new KinematicData();
            this.Target.position = hit.point + hit.normal * this.AvoidMargin;

            return base.GetMovement();
        }
    }

}
