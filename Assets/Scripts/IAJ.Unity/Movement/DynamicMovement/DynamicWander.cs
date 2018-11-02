using Assets.Scripts.IAJ.Unity.Util;
namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicWander : DynamicSeek
    {
        public override string Name
        {
            get { return "Wander"; }
        }
        public float TurnAngle { get; private set; }
        public float WanderOffset { get; private set; }
        public float WanderRadius { get; private set; }
        protected float WanderOrientation { get; set; }

        public DynamicWander()
        {
            this.Target = new KinematicData();
            this.TurnAngle = 20;
            this.WanderOffset = 5;
            this.WanderRadius = 10;
        }

        public override MovementOutput GetMovement()
        {
            
            this.WanderOrientation += RandomHelper.RandomBinomial() * TurnAngle;
            this.Target.orientation = this.WanderOrientation + this.Character.orientation;
            var circleCenter = this.Character.position + WanderOffset * this.Character.GetOrientationAsVector();
            this.Target.position = circleCenter + WanderRadius * this.Target.GetOrientationAsVector();

            return base.GetMovement();
        }
    }
}
