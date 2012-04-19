using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Classes
{
    class Environment3
    {
        private Hole hole;
        private Ball ball;
        private User user;
        private float wind;
        private float friction;


        //public int IsSolvable()
        //{

        //    if (ball.Radius <= 0)
        //        return Constants3.NEGATIVE_BRADIUS;
        //    if (ball.Mass <= 0)
        //        return Constants3.NEGATIVE_BMASS;
        //    if (hole.Radius <= 0)
        //        return Constants3.NEGATIVE_HRADIUS;
        //    if (user.LegMass <= 0)
        //        return Constants3.NEGATIVE_LMASS;
        //    //hole position not before the leg position
        //    if (hole.Position.Y - user.ShootingPosition.Y <= 0)
        //        return Constants3.NEGATIVE_HPOSY;
        //    if (friction < 0)
        //        return Constants3.NEGATIVE_FRICTION;
        //    if (ball.Radius > hole.Radius)
        //        return Constants3.NEGATIVE_RDIFFERENCE;

        //    /*
        //     * Enviornment e;
        //     * e.getFinalPosition(e.getVelocityAfterCollision(new Vector2(Constants3.MAX_VELOCITYX, Constants3.MAX_VELOCITYY,
        //     * ballMass,legMass,wind));//implement with hasScored();
        //    */

        //    Vector2 finalPos = Vector2.Zero;//to be changed with the position returned from getFinalPostion();
        //    if (Vector2.Subtract(finalPos, user.ShootingPosition).LengthSquared() > Vector2.Subtract(hole.Position, user.ShootingPosition).LengthSquared())
        //        return Constants3.HOLE_OUT_OF_FAR_RANGE;
        //    /*
        //     * Enviornment e;
        //     * e.getFinalPosition(e.getVelocityAfterCollision(new Vector2(Constants3.MIN_VELOCITYX, Constants3.MIN_VELOCITYY,
        //     * ballMass, legMass, wind, friction));
        //    */

        //    if (Vector2.Subtract(finalPos, user.ShootingPosition).LengthSquared() > Vector2.Subtract(hole.Position, user.ShootingPosition).LengthSquared()) //length squared used for better performance than length
        //        return Constants3.HOLE_OUT_OF_NEAR_RANGE;

        //    return Constants3.SOLVABLE_EXPERIMENT;//means solvable
        //}


        //public void GenerateSolvable()
        //{
        //    int x = Constants3.SOLVABLE_EXPERIMENT;
        //    do
        //    {
        //        x = IsSolvable();
        //        switch (x)
        //        {
        //            case Constants3.HOLE_OUT_OF_NEAR_RANGE: if (hole.Position.X == Constants3.MAX_HOLEPOSX && hole.Position.Y != Constants3.MAX_HOLEPOSY)
        //                    hole.Position = (Vector2.Add(hole.Position, new Vector2(0, -1)));
        //                else if (hole.Position.Y == Constants3.MAX_HOLEPOSX && hole.Position.X != Constants3.MAX_HOLEPOSY)
        //                    hole.Position = (Vector2.Add(hole.Position, new Vector2(1, 0)))
        //                else friction++;break;
        //            case Constants3.HOLE_OUT_OF_FAR_RANGE: if (hole.Position.X == Constants3.MAX_HOLEPOSX && hole.Position.Y != Constants3.MAX_HOLEPOSY)
        //                    hole.Position = (Vector2.Subtract(hole.Position, new Vector2(0, -1)));
        //                else if (hole.Position.Y == Constants3.MAX_HOLEPOSX && hole.Position.X != Constants3.MAX_HOLEPOSY)
        //                    hole.Position = (Vector2.Subtract(hole.Position, new Vector2(1, 0)));
        //                else if (wind != 0)
        //                    wind--;
        //                else if (friction != 1)
        //                    friction--;    
        //                else if(friction <= 1)
        //                    friction-=0.1f;
        //                break;
        //            case Constants3.NEGATIVE_RDIFFERENCE: int tmp = ball.Radius; ball.Radius = (hole.Radius); hole.Radius = (tmp); break;
        //            case Constants3.NEGATIVE_LMASS: user.LegMass *=1 ; break;
        //            case Constants3.NEGATIVE_BMASS: ball.Mass*= -1; break;
        //            case Constants3.NEGATIVE_BRADIUS: ball.Radius*= -1; break;
        //            case Constants3.NEGATIVE_HRADIUS: hole.Radius*= -1; break;
        //            case Constants3.NEGATIVE_FRICTION: friction*=-1; break;
        //        }
        //    } while ((x = IsSolvable()) != Constants3.SOLVABLE_EXPERIMENT);
        //}
    }
}
