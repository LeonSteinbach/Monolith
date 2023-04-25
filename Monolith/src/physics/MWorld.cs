using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monolith.math;

namespace Monolith.physics;

public class MWorld
{
	private readonly List<MBody> bodies;
	private readonly List<(MBody, MBody)> contactPairs;

	private readonly Vector2[] contactList;
	private readonly Vector2[] impulseList;
	private readonly Vector2[] raList;
	private readonly Vector2[] rbList;
	private readonly Vector2[] frictionImpulseList;
	private readonly float[] jList;
	
	public Vector2 Gravity { get; set; }

	public MWorld()
	{
		Gravity = new Vector2(0f, 1000.81f);
		
		bodies = new List<MBody>();
		contactPairs = new List<(MBody, MBody)>();
		
		contactList = new Vector2[2];
		impulseList = new Vector2[2];
		raList = new Vector2[2];
		rbList = new Vector2[2];
		frictionImpulseList = new Vector2[2];
		jList = new float[2];
	}

	public void AddBody(MBody body)
	{
		body.Gravity = Gravity;
		bodies.Add(body);
	}

	public void RemoveBody(MBody body)
	{
		bodies.Remove(body);
	}

	public bool GetBody(int index, out MBody body)
	{
		body = null;
		if (index < 0 || index >= bodies.Count)
			return false;
		body = bodies[index];
		return true;
	}

	public void Update(GameTime gameTime)
	{
		contactPairs.Clear();
		UpdateBodies(gameTime);
		BroadPhase();
		NarrowPhase();
	}

	public void Render(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime gameTime)
	{
		foreach (var body in bodies)
			body.Render(graphics, spriteBatch, gameTime);
	}

	private void UpdateBodies(GameTime gameTime)
	{
		foreach (var body in bodies)
			body.Update(gameTime);
	}

	private void BroadPhase()
	{
		foreach (var bodyA in bodies)
		{
			Rectangle bodyAbb = bodyA.Hitbox.BoundingBox;
			
			foreach (var bodyB in bodies)
			{
				if (bodyA == bodyB)
					continue;
				
				if (bodyA.IsStatic && bodyB.IsStatic)
					continue;
				
				Rectangle bodyBbb = bodyA.Hitbox.BoundingBox;
				if (!bodyAbb.Intersects(bodyBbb))
					continue;

				contactPairs.Add((bodyA, bodyB));
			}
		}
	}

	private void NarrowPhase()
	{
		foreach (var (bodyA, bodyB) in contactPairs)
		{
			if (MPhysicsHelper.Intersect(bodyA.Hitbox, bodyB.Hitbox, out Vector2 normal, out float depth))
			{
				SeparateBodies(bodyA, bodyB, normal, depth);
				MPhysicsHelper.FindContactPoints(bodyA.Hitbox, bodyB.Hitbox, out Vector2 contact1, out Vector2 contact2, out int contactCount);
				
				ResolveCollision(bodyA, bodyB, normal);
				//ResolveCollisionWithRotation(bodyA, bodyB, normal, contact1, contact2, contactCount);
				//ResolveCollisionWithRotationAndFriction(bodyA, bodyB, normal, contact1, contact2, contactCount);
			}
		}
	}

	private void SeparateBodies(MBody bodyA, MBody bodyB, Vector2 normal, float depth)
	{
		Vector2 mtv = normal * depth * 1.0f;
		
		if (bodyA.IsStatic)
			bodyB.Move(mtv);
		else if (bodyB.IsStatic)
			bodyA.Move(-mtv);
		else
		{
			bodyA.Move(-mtv / 2f);
			bodyB.Move(mtv / 2f);
		}
	}
	
	public void ResolveCollision(MBody bodyA, MBody bodyB, Vector2 normal)
	{
		Vector2 relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;

		if (Vector2.Dot(relativeVelocity, normal) > 0f)
			return;

		float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

		float j = -(1f + e) * Vector2.Dot(relativeVelocity, normal);
		
		float divisor = bodyA.IsStatic ? 0f : bodyA.InverseMass;
		divisor += bodyB.IsStatic ? 0f : bodyB.InverseMass;
		if (divisor != 0)
			j /= divisor;

		Vector2 impulse = j * normal;

		if (!bodyA.IsStatic)
			bodyA.LinearVelocity -= impulse * bodyA.InverseMass;
		if (!bodyB.IsStatic)
			bodyB.LinearVelocity += impulse * bodyB.InverseMass;
	}
	
	public void ResolveCollisionWithRotation(MBody bodyA, MBody bodyB, Vector2 normal, Vector2 contact1, Vector2 contact2, int contactCount)
	{
		float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

        float sf = (bodyA.StaticFriction + bodyB.StaticFriction) * 0.5f;
        float df = (bodyA.DynamicFriction + bodyB.DynamicFriction) * 0.5f;

        contactList[0] = contact1;
        contactList[1] = contact2;

        for (int i = 0; i < contactCount; i++)
        {
            impulseList[i] = Vector2.Zero;
            raList[i] = Vector2.Zero;
            rbList[i] = Vector2.Zero;
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 ra = contactList[i] - bodyA.Hitbox.CenterOfMass;
            Vector2 rb = contactList[i] - bodyB.Hitbox.CenterOfMass;

            raList[i] = ra;
            rbList[i] = rb;

            Vector2 raPerp = new Vector2(-ra.Y, ra.X);
            Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

            Vector2 angularLinearVelocityA = raPerp * bodyA.RotationalVelocity;
            Vector2 angularLinearVelocityB = rbPerp * bodyB.RotationalVelocity;

            Vector2 relativeVelocity =
                (bodyB.LinearVelocity + angularLinearVelocityB) -
                (bodyA.LinearVelocity + angularLinearVelocityA);

            float contactVelocityMag = Vector2.Dot(relativeVelocity, normal);

            if (contactVelocityMag > 0f)
            {
                continue;
            }

            float raPerpDotN = Vector2.Dot(raPerp, normal);
            float rbPerpDotN = Vector2.Dot(rbPerp, normal);

            float divisor = bodyA.IsStatic ? 0f : bodyA.InverseMass;
            divisor += bodyB.IsStatic ? 0f : bodyB.InverseMass;
            divisor += (raPerpDotN * raPerpDotN) * bodyA.InverseInertia;
            divisor += (rbPerpDotN * rbPerpDotN) * bodyB.InverseInertia;

            float j = -(1f + e) * contactVelocityMag;
            if (divisor != 0)
            {
	            j /= divisor;
	            j /= contactCount;
            }

            Vector2 impulse = j * normal;
            impulseList[i] = impulse;
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 impulse = impulseList[i];
            Vector2 ra = raList[i];
            Vector2 rb = rbList[i];

            if (!bodyA.IsStatic)
            {
	            bodyA.LinearVelocity += -impulse * bodyA.InverseMass;
	            bodyA.RotationalVelocity += -MMathHelper.Cross(ra, impulse) * bodyA.InverseInertia;
            }

            if (!bodyB.IsStatic)
            {
	            bodyB.LinearVelocity += impulse * bodyB.InverseMass;
	            bodyB.RotationalVelocity += MMathHelper.Cross(rb, impulse) * bodyB.InverseInertia;
            }
        }
	}

	public void ResolveCollisionWithRotationAndFriction(MBody bodyA, MBody bodyB, Vector2 normal, Vector2 contact1, Vector2 contact2, int contactCount)
	{
		float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

        float sf = (bodyA.StaticFriction + bodyB.StaticFriction) * 0.5f;
        float df = (bodyA.DynamicFriction + bodyB.DynamicFriction) * 0.5f;

        contactList[0] = contact1;
        contactList[1] = contact2;

        for (int i = 0; i < contactCount; i++)
        {
            impulseList[i] = Vector2.Zero;
            raList[i] = Vector2.Zero;
            rbList[i] = Vector2.Zero;
            frictionImpulseList[i] = Vector2.Zero;
            jList[i] = 0f;
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 ra = contactList[i] - bodyA.Hitbox.CenterOfMass;
            Vector2 rb = contactList[i] - bodyB.Hitbox.CenterOfMass;

            raList[i] = ra;
            rbList[i] = rb;

            Vector2 raPerp = new Vector2(-ra.Y, ra.X);
            Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

            Vector2 angularLinearVelocityA = raPerp * bodyA.RotationalVelocity;
            Vector2 angularLinearVelocityB = rbPerp * bodyB.RotationalVelocity;

            Vector2 relativeVelocity =
                (bodyB.LinearVelocity + angularLinearVelocityB) -
                (bodyA.LinearVelocity + angularLinearVelocityA);

            float contactVelocityMag = Vector2.Dot(relativeVelocity, normal);

            if (contactVelocityMag > 0f)
                continue;

            float raPerpDotN = Vector2.Dot(raPerp, normal);
            float rbPerpDotN = Vector2.Dot(rbPerp, normal);

            float divisor = bodyA.IsStatic ? 0f : bodyA.InverseMass;
            divisor += bodyB.IsStatic ? 0f : bodyB.InverseMass;
            divisor += (raPerpDotN * raPerpDotN) * bodyA.InverseInertia;
            divisor += (rbPerpDotN * rbPerpDotN) * bodyB.InverseInertia;

            float j = -(1f + e) * contactVelocityMag;
            if (divisor != 0)
            {
	            j /= divisor;
	            j /= contactCount;
            }

            jList[i] = j;

            Vector2 impulse = j * normal;
            impulseList[i] = impulse;
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 impulse = impulseList[i];
            Vector2 ra = raList[i];
            Vector2 rb = rbList[i];

            if (!bodyA.IsStatic)
            {
	            bodyA.LinearVelocity += -impulse * bodyA.InverseMass;
	            bodyA.RotationalVelocity += -MMathHelper.Cross(ra, impulse) * bodyA.InverseInertia;
            }

            if (!bodyB.IsStatic)
            {
	            bodyB.LinearVelocity += impulse * bodyB.InverseMass;
	            bodyB.RotationalVelocity += MMathHelper.Cross(rb, impulse) * bodyB.InverseInertia;
            }
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 ra = contactList[i] - bodyA.Hitbox.CenterOfMass;
            Vector2 rb = contactList[i] - bodyB.Hitbox.CenterOfMass;

            raList[i] = ra;
            rbList[i] = rb;

            Vector2 raPerp = new Vector2(-ra.Y, ra.X);
            Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

            Vector2 angularLinearVelocityA = raPerp * bodyA.RotationalVelocity;
            Vector2 angularLinearVelocityB = rbPerp * bodyB.RotationalVelocity;

            Vector2 relativeVelocity =
                (bodyB.LinearVelocity + angularLinearVelocityB) -
                (bodyA.LinearVelocity + angularLinearVelocityA);

            Vector2 tangent = relativeVelocity - Vector2.Dot(relativeVelocity, normal) * normal;

            if (MMathHelper.NearlyEqual(tangent, Vector2.Zero))
            {
                continue;
            }
            tangent.Normalize();

            float raPerpDotT = Vector2.Dot(raPerp, tangent);
            float rbPerpDotT = Vector2.Dot(rbPerp, tangent);
            
            float divisor = bodyA.IsStatic ? 0f : bodyA.InverseMass;
            divisor += bodyB.IsStatic ? 0f : bodyB.InverseMass;
            divisor += (raPerpDotT * raPerpDotT) * bodyA.InverseInertia;
            divisor += (rbPerpDotT * rbPerpDotT) * bodyB.InverseInertia;

            float jt = -Vector2.Dot(relativeVelocity, tangent);
            if (divisor != 0)
            {
	            jt /= divisor;
	            jt /= contactCount;
            }

            Vector2 frictionImpulse;
            float j = jList[i];

            if(MathF.Abs(jt) <= j * sf)
                frictionImpulse = jt * tangent;
            else
                frictionImpulse = -j * tangent * df;

            frictionImpulseList[i] = frictionImpulse;
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 frictionImpulse = frictionImpulseList[i];
            Vector2 ra = raList[i];
            Vector2 rb = rbList[i];

            if (!bodyA.IsStatic)
            {
	            bodyA.LinearVelocity += -frictionImpulse * bodyA.InverseMass;
	            bodyA.RotationalVelocity += -MMathHelper.Cross(ra, frictionImpulse) * bodyA.InverseInertia;
            }

            if (!bodyB.IsStatic)
            {
	            bodyB.LinearVelocity += frictionImpulse * bodyB.InverseMass;
	            bodyB.RotationalVelocity += MMathHelper.Cross(rb, frictionImpulse) * bodyB.InverseInertia;
            }
        }
	}
}