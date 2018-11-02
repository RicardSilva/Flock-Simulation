using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.IAJ.Unity.Util;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class FlockFlee : DynamicFlee
    {
        public override string Name
        {
            get { return "Flee"; }
        }

        public List<KinematicData> Flock { get; set; }
        public float FleeDistance { get; set; }
        public bool Active = false;


        public FlockFlee()
        {
            //TODO: TWEAK VARIABLES
            this.FleeDistance = 20.0f;
            BoidsManager.OnMouseClicked += Activate;
        }

        public override MovementOutput GetMovement()
        {
            if (this.Active == false)
                return null;

            if ((this.Character.position - this.Target.position).sqrMagnitude < this.FleeDistance * this.FleeDistance) {
                return base.GetMovement();
            }

            this.Active = false;
            return null;
        }

        public void Activate(Vector3 clickPosition) {
            this.Active = true;
            this.Target.position = clickPosition;       
        }

    }
}