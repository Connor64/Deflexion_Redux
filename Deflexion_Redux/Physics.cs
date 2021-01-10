using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {

    public enum ForceMode { Normal, Impulsive };
    class Physics {

        public Vector2 acceleration;
        private Vector2 force = Vector2.Zero;
        //private float torque;
        private float velocityThreshold = 8f;        // If the object's velocity is within this threshold +/-, it is set to 0
        public Vector2 velocity = Vector2.Zero;

        // Values below are defaults and are meant to be changed in the constructor of whatever classes that inherit from this one.
        public float mass = 1f;
        public float collisionBoxSize = 32f;        // Size of the object's collider in pixels (unscaled)
        public Vector2 position = Vector2.Zero;
        public bool instantaneous = false;          // If true, object will stop immediately if no forces are applied
        public Vector2 boundary = Vector2.Zero;     // The object's physical boundary (matching the screen). If left at default value then it will be ignored

        public float resistance = 400f;             // The "air" resistance that the object faces
        public float baseSpeedLimit = 500f;         // Base speed limit for the speedLimit value to return to when boosting (does not ever change)
        public float speedLimit = 500f;             // Limits the object's speed (increased when launch occurs)
        public float boostFalloffSpeed = 1000f;     // The speed at which the launch boost speed limit increase shrinks

        public bool player = false;                 // If true, the object will interact with tiles (probably will be changed in the future)

        public void addForce(Vector2 direction, float strength, float launchLimit) {
            force += direction * strength;
            if (launchLimit > 0) {
                speedLimit = launchLimit;
            }
        }

        //public void addTorque(float torque) {

        //}

        public void PhysicsUpdate(float deltaTime, List<Sprite> tiles) {
            acceleration = force / mass;

            velocity += acceleration * deltaTime; // v = vi + at

            if (velocity.Length() > speedLimit) {
                velocity = Vector2.Normalize(velocity) * speedLimit;
            }

            if (player) {
                position = playerCollision(position + velocity * deltaTime, tiles);
            } else {
                position += velocity * deltaTime;
            }

            if (!instantaneous && force == Vector2.Zero) {

                if (velocity.X > 0) {
                    velocity.X -= (resistance + speedLimit - baseSpeedLimit) * deltaTime;
                } else if (velocity.X < 0) {
                    velocity.X += (resistance + speedLimit - baseSpeedLimit) * deltaTime;
                }

                if (velocity.Y > 0) {
                    velocity.Y -= (resistance + speedLimit - baseSpeedLimit) * deltaTime;
                } else if (velocity.Y < 0) {
                    velocity.Y += (resistance + speedLimit - baseSpeedLimit) * deltaTime;
                }

                if (MathF.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y) < velocityThreshold) {
                    velocity = Vector2.Zero;
                }

            } else if (instantaneous && acceleration == Vector2.Zero) {
                velocity = Vector2.Zero;
            }

            if (speedLimit > baseSpeedLimit) {
                speedLimit -= boostFalloffSpeed * deltaTime;
            }
            if (speedLimit < baseSpeedLimit) {
                speedLimit = baseSpeedLimit;
            }

            force = Vector2.Zero;
            acceleration = Vector2.Zero;
        }

        Vector2 playerCollision(Vector2 newPosition, List<Sprite> tiles) {
            Vector2 nextPosition = newPosition;

            Vector2 nextPosition_X = new Vector2(newPosition.X, position.Y);
            Vector2 nextPosition_Y = new Vector2(position.X, newPosition.Y);

            foreach (Sprite tile in tiles) {

                if ((nextPosition_X.Y < tile.Position.Y + tile.Texture.Height * 2 &&
                    nextPosition_X.Y + collisionBoxSize > tile.Position.Y &&
                    nextPosition_X.X < tile.Position.X + tile.Texture.Width * 2 &&
                    nextPosition_X.X + collisionBoxSize > tile.Position.X)) {
                    nextPosition.X = position.X;
                    velocity.X = 0;
                }

                if (nextPosition_Y.Y < tile.Position.Y + tile.Texture.Height * 2 &&
                    nextPosition_Y.Y + collisionBoxSize > tile.Position.Y &&
                    nextPosition_Y.X < tile.Position.X + tile.Texture.Width * 2 &&
                    nextPosition_Y.X + collisionBoxSize > tile.Position.X) {
                    nextPosition.Y = position.Y;
                    velocity.Y = 0;
                }

                //if (boundary != Vector2.Zero) {
                //    if (nextPosition.X > boundary.X || nextPosition.X < 0) {
                //        nextPosition.X = position.X;
                //        velocity.X = 0;
                //    }
                //    if (nextPosition.Y > boundary.Y || nextPosition.Y < 0) {
                //        nextPosition.Y = position.Y;
                //        velocity.Y = 0;
                //    }
                //}
            }
            return nextPosition;
        }
    }
}