// ILSpyBased#2
using System;
using UnityEngine;

namespace RootMotion.FinalIK
{
    public class HitReactionVRIK : OffsetModifierVRIK
    {
        [Serializable]
        public abstract class Offset
        {
            [Tooltip("Just for visual clarity, not used at all")]
            public string name;

            [Tooltip("Linking this hit point to a collider")]
            public Collider collider;

            [Tooltip("Only used if this hit point gets hit when already processing another hit")]
            [SerializeField]
            private float crossFadeTime = 0.1f;

            private float length;

            private float crossFadeSpeed;

            private float lastTime;

            protected float crossFader
            {
                get;
                private set;
            }

            protected float timer
            {
                get;
                private set;
            }

            protected Vector3 force
            {
                get;
                private set;
            }

            protected Vector3 point
            {
                get;
                private set;
            }

            public void Hit(Vector3 force, AnimationCurve[] curves, Vector3 point)
            {
                if (this.length == 0f)
                {
                    this.length = this.GetLength(curves);
                }
                if (this.length <= 0f)
                {
                    UnityEngine.Debug.LogError("Hit Point WeightCurve length is zero.");
                }
                else
                {
                    if (this.timer < 1f)
                    {
                        this.crossFader = 0f;
                    }
                    this.crossFadeSpeed = ((!(this.crossFadeTime > 0f)) ? 0f : (1f / this.crossFadeTime));
                    this.CrossFadeStart();
                    this.timer = 0f;
                    this.force = force;
                    this.point = point;
                }
            }

            public void Apply(VRIK ik, AnimationCurve[] curves, float weight)
            {
                float num = Time.time - this.lastTime;
                this.lastTime = Time.time;
                if (!(this.timer >= this.length))
                {
                    this.timer = Mathf.Clamp(this.timer + num, 0f, this.length);
                    if (this.crossFadeSpeed > 0f)
                    {
                        this.crossFader = Mathf.Clamp(this.crossFader + num * this.crossFadeSpeed, 0f, 1f);
                    }
                    else
                    {
                        this.crossFader = 1f;
                    }
                    this.OnApply(ik, curves, weight);
                }
            }

            protected abstract float GetLength(AnimationCurve[] curves);

            protected abstract void CrossFadeStart();

            protected abstract void OnApply(VRIK ik, AnimationCurve[] curves, float weight);
        }

        [Serializable]
        public class PositionOffset : Offset
        {
            [Serializable]
            public class PositionOffsetLink
            {
                [Tooltip("The FBBIK effector type")]
                public IKSolverVR.PositionOffset positionOffset;

                [Tooltip("The weight of this effector (could also be negative)")]
                public float weight;

                private Vector3 lastValue;

                private Vector3 current;

                public void Apply(VRIK ik, Vector3 offset, float crossFader)
                {
                    this.current = Vector3.Lerp(this.lastValue, offset * this.weight, crossFader);
                    ik.solver.AddPositionOffset(this.positionOffset, this.current);
                }

                public void CrossFadeStart()
                {
                    this.lastValue = this.current;
                }
            }

            [Tooltip("Offset magnitude in the direction of the hit force")]
            public int forceDirCurveIndex;

            [Tooltip("Offset magnitude in the direction of character.up")]
            public int upDirCurveIndex = 1;

            [Tooltip("Linking this offset to the VRIK position offsets")]
            public PositionOffsetLink[] offsetLinks;

            protected override float GetLength(AnimationCurve[] curves)
            {
                float num = (curves[this.forceDirCurveIndex].keys.Length <= 0) ? 0f : curves[this.forceDirCurveIndex].keys[curves[this.forceDirCurveIndex].length - 1].time;
                float min = (curves[this.upDirCurveIndex].keys.Length <= 0) ? 0f : curves[this.upDirCurveIndex].keys[curves[this.upDirCurveIndex].length - 1].time;
                return Mathf.Clamp(num, min, num);
            }

            protected override void CrossFadeStart()
            {
                PositionOffsetLink[] array = this.offsetLinks;
                foreach (PositionOffsetLink positionOffsetLink in array)
                {
                    positionOffsetLink.CrossFadeStart();
                }
            }

            protected override void OnApply(VRIK ik, AnimationCurve[] curves, float weight)
            {
                Vector3 a = ik.transform.up * base.force.magnitude;
                Vector3 a2 = curves[this.forceDirCurveIndex].Evaluate(base.timer) * base.force + curves[this.upDirCurveIndex].Evaluate(base.timer) * a;
                a2 *= weight;
                PositionOffsetLink[] array = this.offsetLinks;
                foreach (PositionOffsetLink positionOffsetLink in array)
                {
                    positionOffsetLink.Apply(ik, a2, base.crossFader);
                }
            }
        }

        [Serializable]
        public class RotationOffset : Offset
        {
            [Serializable]
            public class RotationOffsetLink
            {
                [Tooltip("Reference to the bone that this hit point rotates")]
                public IKSolverVR.RotationOffset rotationOffset;

                [Tooltip("Weight of rotating the bone")]
                [Range(0f, 1f)]
                public float weight;

                private Quaternion lastValue = Quaternion.identity;

                private Quaternion current = Quaternion.identity;

                public void Apply(VRIK ik, Quaternion offset, float crossFader)
                {
                    this.current = Quaternion.Lerp(this.lastValue, Quaternion.Lerp(Quaternion.identity, offset, this.weight), crossFader);
                    ik.solver.AddRotationOffset(this.rotationOffset, this.current);
                }

                public void CrossFadeStart()
                {
                    this.lastValue = this.current;
                }
            }

            [Tooltip("The angle to rotate the bone around it's rigidbody's world center of mass")]
            public int curveIndex;

            [Tooltip("Linking this hit point to bone(s)")]
            public RotationOffsetLink[] offsetLinks;

            private Rigidbody rigidbody;

            protected override float GetLength(AnimationCurve[] curves)
            {
                return (curves[this.curveIndex].keys.Length <= 0) ? 0f : curves[this.curveIndex].keys[curves[this.curveIndex].length - 1].time;
            }

            protected override void CrossFadeStart()
            {
                RotationOffsetLink[] array = this.offsetLinks;
                foreach (RotationOffsetLink rotationOffsetLink in array)
                {
                    rotationOffsetLink.CrossFadeStart();
                }
            }

            protected override void OnApply(VRIK ik, AnimationCurve[] curves, float weight)
            {
                if ((UnityEngine.Object)base.collider == (UnityEngine.Object)null)
                {
                    UnityEngine.Debug.LogError("No collider assigned for a HitPointBone in the HitReaction component.");
                }
                else
                {
                    if ((UnityEngine.Object)this.rigidbody == (UnityEngine.Object)null)
                    {
                        this.rigidbody = ((Component)base.collider).GetComponent<Rigidbody>();
                    }
                    if ((UnityEngine.Object)this.rigidbody != (UnityEngine.Object)null)
                    {
                        Vector3 axis = Vector3.Cross(base.force, base.point - this.rigidbody.worldCenterOfMass);
                        float angle = curves[this.curveIndex].Evaluate(base.timer) * weight;
                        Quaternion offset = Quaternion.AngleAxis(angle, axis);
                        RotationOffsetLink[] array = this.offsetLinks;
                        foreach (RotationOffsetLink rotationOffsetLink in array)
                        {
                            rotationOffsetLink.Apply(ik, offset, base.crossFader);
                        }
                    }
                }
            }
        }

        public AnimationCurve[] offsetCurves;

        [Tooltip("Hit points for the FBBIK effectors")]
        public PositionOffset[] positionOffsets;

        [Tooltip(" Hit points for bones without an effector, such as the head")]
        public RotationOffset[] rotationOffsets;

        protected override void OnModifyOffset()
        {
            PositionOffset[] array = this.positionOffsets;
            foreach (PositionOffset positionOffset in array)
            {
                positionOffset.Apply(base.ik, this.offsetCurves, base.weight);
            }
            RotationOffset[] array2 = this.rotationOffsets;
            foreach (RotationOffset rotationOffset in array2)
            {
                rotationOffset.Apply(base.ik, this.offsetCurves, base.weight);
            }
        }

        public void Hit(Collider collider, Vector3 force, Vector3 point)
        {
            if ((UnityEngine.Object)base.ik == (UnityEngine.Object)null)
            {
                UnityEngine.Debug.LogError("No IK assigned in HitReaction");
            }
            else
            {
                PositionOffset[] array = this.positionOffsets;
                foreach (PositionOffset positionOffset in array)
                {
                    if ((UnityEngine.Object)positionOffset.collider == (UnityEngine.Object)collider)
                    {
                        positionOffset.Hit(force, this.offsetCurves, point);
                    }
                }
                RotationOffset[] array2 = this.rotationOffsets;
                foreach (RotationOffset rotationOffset in array2)
                {
                    if ((UnityEngine.Object)rotationOffset.collider == (UnityEngine.Object)collider)
                    {
                        rotationOffset.Hit(force, this.offsetCurves, point);
                    }
                }
            }
        }
    }
}


