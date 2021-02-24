using Maze.Library;
using System.Collections.Generic;
using System.Linq;

namespace Maze.Solver
{
    /// <summary>
    /// Moves a robot from its current position towards the exit of the maze
    /// </summary>
    public class RobotController
    {
        private readonly IRobot robot;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotController"/> class
        /// </summary>
        /// <param name="robot">Robot that is controlled</param>
        public RobotController(IRobot robot)
        {
            // Store robot for later use
            this.robot = robot;
        }

        /// <summary>
        /// Moves the robot to the exit
        /// </summary>
        /// <remarks>
        /// This function uses methods of the robot that was passed into this class'
        /// constructor. It has to move the robot until the robot's event
        /// <see cref="IRobot.ReachedExit"/> is fired. If the algorithm finds out that
        /// the exit is not reachable, it has to call <see cref="IRobot.HaltAndCatchFire"/>
        /// and exit.
        /// </remarks>
        public void MoveRobotToExit()
        {
            // Here you have to add your code
            Direction direction;
            bool move = canIMove(out direction);
            // Trivial sample algorithm that can just move right
            var reachedEnd = false;
            robot.ReachedExit += (_, __) => reachedEnd = true;


            

            while (!reachedEnd && move)
            {
                switch (direction)
                {
                    case Direction.Left:
                        robot.Move(Direction.Left);
                        break;

                    case Direction.Up:
                        robot.Move(Direction.Up);
                        break;

                    case Direction.Right:
                        robot.Move(Direction.Right);
                        break;

                    case Direction.Down:
                        robot.Move(Direction.Down);
                        break;
                }

                move = robot.CanIMove(direction) ? true : canIMove2(ref direction);
            }
        }




        public Direction MirrorDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Right,
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                _ => throw new System.Exception()
            };
        }

        public bool canIMove2(ref Direction direction)
        {
            List<Direction> d = new List<Direction>()
            {
                Direction.Left,
                Direction.Up,
                Direction.Right,
                Direction.Down
            };

            d.Remove(MirrorDirection(direction));

            foreach (var item in d)
            {
                if (robot.CanIMove(item))
                {
                    direction = item;
                    return true;
                }
            }

            robot.HaltAndCatchFire();
            return false;
        }

        public bool canIMove(out Direction direction)
        {
            if (robot.CanIMove(Direction.Left))
            {
                direction = Direction.Left;
                return true;
            }
            else if (robot.CanIMove(Direction.Up))
            {
                direction = Direction.Up;
                return true;
            }
            else if (robot.CanIMove(Direction.Right))
            {
                direction = Direction.Right;
                return true;
            }
            else if (robot.CanIMove(Direction.Down))
            {
                direction = Direction.Down;
                return true;
            }

            robot.HaltAndCatchFire();
            direction = Direction.Left;
            return false;
        }

    }
}
