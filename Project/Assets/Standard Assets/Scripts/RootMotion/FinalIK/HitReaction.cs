// ILSpyBased#2
using System;
using UnityEngine;

namespace RootMotion.FinalIK
{
    public class HitReaction : OffsetModifier
    {
        [Serializable]
        public abstract class HitPoint
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

            public void Hit(Vector3 force, Vector3 point)
            {
                if (this.length == 0f)
                {
                    this.length = this.GetLength();
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

            public void Apply(IKSolverFullBodyBiped solver, float weight)
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
                    this.OnApply(solver, weight);
                }
            }

            protected abstract float GetLength();

            protected abstract void CrossFadeStart();

            protected abstract void OnApply(IKSolverFullBodyBiped solver, float weight);
        }

        [Serializable]
        public class HitPointEffector : HitPoint
        {
            [Serializable]
            public class EffectorLink
            {
                [Tooltip("The FBBIK effector type")]
                public FullBodyBipedEffector effector;

                [Tooltip("The weight of this effector (could also be negative)")]
                public float weight;

                private Vector3 lastValue;

                private Vector3 current;

                public void Apply(IKSolverFullBodyBiped solver, Vector3 offset, float crossFader)
                {
                    this.current = Vector3.Lerp(this.lastValue, offset * this.weight, crossFader);
                    IKEffector iKEffector = solver.GetEffector(this.effector);
                    iKEffector.positionOffset += this.current;
                }

                public void CrossFadeStart()
                {
                    this.lastValue = this.current;
                }
            }

            [Tooltip("Offset magnitude in the direction of the hit force")]
            public AnimationCurve offsetInForceDirection;

            [Tooltip("Offset magnitude in the direction of character.up")]
            public AnimationCurve offsetInUpDirection;

            [Tooltip("Linking this offset to the FBBIK effectors")]
            public EffectorLink[] effectorLinks;

            protected override float GetLength()
            {
                float num = (this.offsetInForceDirection.keys.Length <= 0) ? 0f : this.offsetInForceDirection.keys[this.offsetInForceDirection.length - 1].time;
                float min = (this.offsetInUpDirection.keys.Length <= 0) ? 0f : this.offsetInUpDirection.keys[this.offsetInUpDirection.length - 1].time;
                return Mathf.Clamp(num, min, num);
            }

            protected override void CrossFadeStart()
            {
                EffectorLink[] array = this.effectorLinks;
                foreach (EffectorLink effectorLink in array)
                {
                    effectorLink.CrossFadeStart();
                }
            }

            protected override void OnApply(IKSolverFullBodyBiped solver, float weight)
            {
                Vector3 a = solver.GetRoot().up * base.force.magnitude;
                Vector3 a2 = this.offsetInForceDirection.Evaluate(base.timer) * base.force + this.offsetInUpDirection.Evaluate(base.timer) * a;
                a2 *= weight;
                EffectorLink[] array = this.effectorLinks;
                foreach (EffectorLink effectorLink in array)
                {
                    effectorLink.Apply(solver, a2, base.crossFader);
                }
            }
        }

        [Serializable]
        public class HitPointBone : HitPoint
        {
            [Serializable]
            public class BoneLink
            {
                [Tooltip("Reference to the bone that this hit point rotates")]
                public Transform bone;

                [Tooltip("Weight of rotating the bone")]
                [Range(0f, 1f)]
                public float weight;

                private Quaternion lastValue = Quaternion.identity;

                private Quaternion current = Quaternion.identity;

                public void Apply(IKSolverFullBodyBiped solver, Quaternion offset, float crossFader)
                {
                    this.current = Quaternion.Lerp(this.lastValue, Quaternion.Lerp(Quaternion.identity, offset, this.weight), crossFader);
                    this.bone.rotation = this.current * this.bone.rotation;
                }

                public void CrossFadeStart()
                {
                    this.lastValue = this.current;
                }
            }

            [Tooltip("The angle to rotate the bone around it's rigidbody's world center of mass")]
            public AnimationCurve aroundCenterOfMass;

            [Tooltip("Linking this hit point to bone(s)")]
            public BoneLink[] boneLinks;

            private Rigidbody rigidbody;

            protected override float GetLength()
            {
                return (this.aroundCenterOfMass.keys.Length <= 0) ? 0f : this.aroundCenterOfMass.keys[this.aroundCenterOfMass.length - 1].time;
            }

            protected override void CrossFadeStart()
            {
                BoneLink[] array = this.boneLinks;
                foreach (BoneLink boneLink in array)
                {
                    boneLink.CrossFadeStart();
                }
            }

            protected override void OnApply(IKSolverFullBodyBiped solver, float weight)
            {
                if ((UnityEngine.Object)this.rigidbody == (UnityEngine.Object)null)
                {
                    this.rigidbody = ((Component)base.collider).GetComponent<Rigidbody>();
                }
                if ((UnityEngine.Object)this.rigidbody != (UnityEngine.Object)null)
                {
                    Vector3 axis = Vector3.Cross(base.force, base.point - this.rigidbody.worldCenterOfMass);
                    float angle = this.aroundCenterOfMass.Evaluate(base.timer) * weight;
                    Quaternion offset = Quaternion.AngleAxis(angle, axis);
                    BoneLink[] array = this.boneLinks;
                    foreach (BoneLink boneLink in array)
                    {
                        boneLink.Apply(solver, offset, base.crossFader);
                    }
                }
            }
        }

        [Tooltip("Hit points for the FBBIK effectors")]
        public HitPointEffector[] effectorHitPoints;

        [Tooltip(" Hit points for bones without an effector, such as the head")]
        public HitPointBone[] boneHitPoints;

        protected override void OnModifyOffset()
        {
            HitPointEffector[] array = this.effectorHitPoints;
            foreach (HitPointEffector hitPointEffector in array)
            {
                hitPointEffector.Apply(base.ik.solver, base.weight);
            }
            HitPointBone[] array2 = this.boneHitPoints;
            foreach (HitPointBone hitPointBone in array2)
            {
                hitPointBone.Apply(base.ik.solver, base.weight);
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
                HitPointEffector[] array = this.effectorHitPoints;
                foreach (HitPointEffector hitPointEffector in array)
                {
                    if ((UnityEngine.Object)hitPointEffector.collider == (UnityEngine.Object)collider)
                    {
                        hitPointEffector.Hit(force, point);
                    }
                }
                HitPointBone[] array2 = this.boneHitPoints;
                foreach (HitPointBone hitPointBone in array2)
                {
                    if ((UnityEngine.Object)hitPointBone.collider == (UnityEngine.Object)collider)
                    {
                        hitPointBone.Hit(force, point);
                    }
                }
            }
        }
    }
}


