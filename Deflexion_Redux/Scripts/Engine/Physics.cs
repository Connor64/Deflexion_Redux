using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Deflexion_Redux {

    public enum BodyType { Player, Tile, Enemy, Bullet };
    public class Physics {

        private Vector2 force = Vector2.Zero;
        //private float torque;
        private float velocityThreshold = 15f;        // If the object's velocity is within this threshold +/-, it is set to 0
        public Vector2 acceleration;
        public Vector2 velocity = Vector2.Zero;

        private Camera cam = Camera.Instance;

        // Values below are defaults and are meant to be changed in the constructor of whatever classes that inherit from this one.
        public BodyType bodyType;
        public float mass = 1f;
        public float collisionBoxSize = 8f;         // Size of the object's collider in pixels (unscaled)
        public Vector2 position = Vector2.Zero;
        public Vector2 previousPosition = Vector2.Zero;
        public bool instantaneous = false;          // If true, object will stop immediately if no forces are applied
        public Vector2 screenTolerance = Vector2.Zero;     // The object's physical boundary (matching the screen). If left at default value then it will be ignored

        public float resistance = 400f;             // The "air" resistance that the object faces
        public float baseSpeedLimit = 500f;         // Base speed limit for the speedLimit value to return to when boosting (does not ever change)
        public float speedLimit = 500f;             // Limits the object's speed (increased when launch occurs)
        public float boostFalloffSpeed = 5000f;     // The speed at which the launch boost speed limit increase shrinks

        public bool movable = true;
        //public bool stuck = false;

        //public List<Tile> tiles = LevelManager.Instance.tiles;
        public Tile[,] tiles = LevelManager.Instance.tiles;

        public void addForce(Vector2 direction, float strength, float launchLimit) {
            if (direction != Vector2.Zero) {
                force += Vector2.Normalize(direction) * strength;
            }
            if (launchLimit > 0) {
                speedLimit = launchLimit;
            }
        }

        public void PhysicsUpdate(float deltaTime) {
            previousPosition = position;

            acceleration = force / mass;

            velocity += acceleration * deltaTime; // v = vi + at

            if (velocity.Length() > speedLimit) {
                velocity = Vector2.Normalize(velocity) * speedLimit;
            }

            if (bodyType == BodyType.Player) {
                position = tileCollision(position + velocity * deltaTime, tiles);
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

            speedLimit = speedLimit < baseSpeedLimit ? baseSpeedLimit : speedLimit - boostFalloffSpeed * deltaTime;

            force = Vector2.Zero;
            acceleration = Vector2.Zero;
        }

        Vector2 tileCollision(Vector2 newPosition, Tile[,] tiles) {
            Vector2 nextPosition = newPosition;

            Vector2 nextPosition_X = new Vector2(newPosition.X, position.Y);
            Vector2 nextPosition_Y = new Vector2(position.X, newPosition.Y);

            foreach (Tile tile in tiles) {
                if (!tile.empty && cam.contains(tile.position, Vector2.Zero) && tile.collidable) {
                    if (nextPosition_X.Y - collisionBoxSize < tile.position.Y + tile.bounds.Y &&
                        nextPosition_X.Y + collisionBoxSize > tile.position.Y &&
                        nextPosition_X.X - collisionBoxSize < tile.position.X + tile.bounds.X &&
                        nextPosition_X.X + collisionBoxSize > tile.position.X) {
                        nextPosition.X = position.X;
                        velocity.X = 0;
                    }

                    if (nextPosition_Y.Y - collisionBoxSize < tile.position.Y + tile.bounds.Y &&
                        nextPosition_Y.Y + collisionBoxSize > tile.position.Y &&
                        nextPosition_Y.X - collisionBoxSize < tile.position.X + tile.bounds.X &&
                        nextPosition_Y.X + collisionBoxSize > tile.position.X) {
                        nextPosition.Y = position.Y;
                        velocity.Y = 0;
                    }
                }
            }
            return nextPosition;
        }

        public bool collisionCheck<T>(Vector2 newPosition, List<T> physicObjects, out T _obj) where T : Physics {
            bool result = false;
            _obj = null;
            foreach (T obj in physicObjects) {
                if (newPosition.Y - collisionBoxSize < obj.position.Y + collisionBoxSize &&
                    newPosition.Y + collisionBoxSize > obj.position.Y - collisionBoxSize &&
                    newPosition.X - collisionBoxSize < obj.position.X + collisionBoxSize &&
                    newPosition.X + collisionBoxSize > obj.position.X - collisionBoxSize) {
                    _obj = obj;
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}