using UnityEngine;
using NPBehave;
using System.Collections.Generic;

namespace Complete
{
    /*
    Example behaviour trees for the Tank AI.  This is partial definition:
    the core AI code is defined in TankAI.cs.

    Use this file to specifiy your new behaviour tree.
     */
    public partial class TankAI : MonoBehaviour
    {
        private Root CreateBehaviourTree() {

            switch (m_Behaviour) {

                case 0:
                    return Easy();
                case 1:
                    return Avoid();
                case 2:
                    return Hard();
			     case 3:
				    return Strange();

                default:
                    return new Root (new Action(()=> Turn(0.1f)));
            }
        }

        /* Actions */

        private Node StopTurning() {
            return new Action(() => Turn(0));
        }
        private Node StopMoving() {
            return new Action(() => Move(0));
        }

        private Node RandomFire() {
            return new Action(() => Fire(UnityEngine.Random.Range(0.0f, 1.0f)));
        }
        private Node AimedFire()
        {
          
            return new Action(() => Fire(0.5f));
        }


        /* Example behaviour trees */

        // Constantly spin and fire on the spot 
        private Root Easy() {
            return new Root(
                new Service(0.2f, UpdatePerception,
                    new Selector(
                       new BlackboardCondition("targetDistance",
                                                Operator.IS_GREATER_OR_EQUAL, 20f,
                                                Stops.IMMEDIATE_RESTART,
                                                new Sequence(StopTurning(), new Action(() => Move(0.5f)))
                                                ),
                        new BlackboardCondition("targetOffCentre",
                                                Operator.IS_SMALLER_OR_EQUAL, 0.1f,
                                                Stops.IMMEDIATE_RESTART,
                             new Sequence(StopTurning(), StopMoving(),
                                        new Wait(0.5f),
                                        RandomFire())),
                         
                         new BlackboardCondition("obstacleFront",
                                         Operator.IS_EQUAL, true,
                                         Stops.IMMEDIATE_RESTART,
                                         new Sequence(new Action(() => StopMoving()), new Action(() => Turn(1f)), new Wait(1f), StopTurning(), new Action(() => Move(-1f)))),
                        new BlackboardCondition("targetOnRight",
                                                Operator.IS_EQUAL, true,
                                                Stops.IMMEDIATE_RESTART,
                            // Turn right toward target
                            new Action(() => Turn(0.8f))),
                            // Turn left toward target
                            new Action(() => Turn(-0.8f))
                    )
                )
            );
        }

        private Root Avoid()
        {
            return new Root(
                new Service(0.2f, UpdatePerception,
                    new Selector(
                         new BlackboardCondition("targetDistance",
                                                Operator.IS_GREATER_OR_EQUAL, 20f,
                                                Stops.IMMEDIATE_RESTART,
                                                new Sequence(StopTurning(), new Action(() => Move(-0.5f)))
                                                ),
                        new BlackboardCondition("targetOffCentre",
                                                Operator.IS_SMALLER_OR_EQUAL, 0.1f,
                                                Stops.IMMEDIATE_RESTART,
                             new Sequence(StopTurning(),
                                        new Wait(0.1f))),
                                         //RandomFire())),
                                         new BlackboardCondition("obstacleBack",
                                         Operator.IS_EQUAL, true,
                                         Stops.IMMEDIATE_RESTART,
                                         new Sequence(new Action(() => StopMoving()), new Action(() => Turn(1f)), new Wait(1f), StopTurning(), new Action(() => Move(1f)))),
                        
                        new BlackboardCondition("targetOnRight",
                                                Operator.IS_EQUAL, true,
                                                Stops.IMMEDIATE_RESTART,
                            // Turn right toward target
                            new Action(() => Turn(0.8f))),
                            // Turn left toward target
                            new Action(() => Turn(-0.8f))
                    )
                )
            );
        }

        private Root Hard()
        {
            return new Root(
                new Service(0.2f, UpdatePerception,
                    new Selector(
                       new BlackboardCondition("targetDistance",
                                                Operator.IS_GREATER_OR_EQUAL, 15f,
                                                Stops.IMMEDIATE_RESTART,
                                                new Sequence(StopTurning(), new Action(() => Move(0.5f)))
                                                ),
                       new BlackboardCondition("targetDistance",
                                                Operator.IS_SMALLER_OR_EQUAL, 10f,
                                                Stops.IMMEDIATE_RESTART,
                                                new Sequence(StopTurning(), new Action(() => Move(-0.5f)))
                                                ),
                        new BlackboardCondition("targetOffCentre",
                                                Operator.IS_SMALLER_OR_EQUAL, 0.1f,
                                                Stops.IMMEDIATE_RESTART,
                             new Sequence(StopTurning(),
                                        new Wait(0.1f),
                                        AimedFire())),

                         new BlackboardCondition("obstacleFront",
                                         Operator.IS_EQUAL, true,
                                         Stops.IMMEDIATE_RESTART,
                                         new Sequence(new Action(() => StopMoving()), new Action(() => Turn(1f)), new Wait(1f), StopTurning(), new Action(() => Move(-1f)))),
                        new BlackboardCondition("targetOnRight",
                                                Operator.IS_EQUAL, true,
                                                Stops.IMMEDIATE_RESTART,
                            // Turn right toward target
                            new Action(() => Turn(0.8f))),
                            // Turn left toward target
                            new Action(() => Turn(-0.8f))
                    )
                )
            );
        }

        private Root Strange()
        {
            return new Root(
                new Service(0.2f, UpdatePerception,
                    new Selector(
                       new BlackboardCondition("targetDistance",
                                                Operator.IS_GREATER_OR_EQUAL, UnityEngine.Random.Range(10f, 20f),
                                                Stops.IMMEDIATE_RESTART,
                                                new Sequence(StopTurning(), new Action(() => Move(UnityEngine.Random.Range(0.1f, 1f))))
                                                ),
                       new BlackboardCondition("targetDistance",
                                                Operator.IS_SMALLER_OR_EQUAL, UnityEngine.Random.Range(1f, 10f),
                                                Stops.IMMEDIATE_RESTART,
                                                new Sequence(StopTurning(), new Action(() => Move(UnityEngine.Random.Range(-0.1f, -1f))))
                                                ),
                        new BlackboardCondition("targetOffCentre",
                                                Operator.IS_SMALLER_OR_EQUAL, 0.1f,
                                                Stops.IMMEDIATE_RESTART,
                             new Sequence(StopTurning(),
                                        new Wait(UnityEngine.Random.Range(0, 1f)),
                                        RandomFire())),

                         new BlackboardCondition("obstacleFront",
                                         Operator.IS_EQUAL, true,
                                         Stops.IMMEDIATE_RESTART,
                                         new Sequence(new Action(() => StopMoving()), new Action(() => Turn(1f)), new Wait(1f), StopTurning(), new Action(() => Move(-1f)))),
                        new BlackboardCondition("targetOnRight",
                                                Operator.IS_EQUAL, true,
                                                Stops.IMMEDIATE_RESTART,
                            // Turn right toward target
                            new Action(() => Turn(UnityEngine.Random.Range(0.1f, 1f)))),
                            // Turn left toward target
                            new Action(() => Turn(UnityEngine.Random.Range(-0.1f, -1f)))
                    )
                )
            );
        }
        // Turn to face your opponent and fire
        private Root TrackBehaviour() {
            return new Root(
                new Service(0.2f, UpdatePerception,
                    new Selector(
                        new BlackboardCondition("targetOffCentre",
                                                Operator.IS_SMALLER_OR_EQUAL, 0.1f,
                                                Stops.IMMEDIATE_RESTART,
                            // Stop turning and fire
                            new Sequence(StopTurning(),
                                        new Wait(2f),
                                        RandomFire())),
                        new BlackboardCondition("targetOnRight",
                                                Operator.IS_EQUAL, true,
                                                Stops.IMMEDIATE_RESTART,
                            // Turn right toward target
                            new Action(() => Turn(0.2f))),
                            // Turn left toward target
                            new Action(() => Turn(-0.2f))
                    )
                )
            );
        }
		private Root TrackAndMove(float shoot){
			return new Root (
				new Service (0.2f, UpdatePerception,
					new Selector (
						new BlackboardCondition ("targetOffCentre",
							Operator.IS_SMALLER_OR_EQUAL, 0.1f,
							Stops.IMMEDIATE_RESTART,
							//stop turning and fire with force 1
							new Sequence (StopTurning (), new Wait (2f), new Action(() => Fire (shoot)))), new BlackboardCondition ("targetOnRight", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,

					new Action (() => Turn (0.2f))),
					new Action (() => Turn (-0.2f))
				)
				)
			);
		
		
		
		}

        private void UpdatePerception() {
            Vector3 targetPos = TargetTransform().position;
            Vector3 localPos = this.transform.InverseTransformPoint(targetPos);
            Vector3 heading = localPos.normalized;
            Vector3 ahead = transform.TransformDirection(Vector3.forward); 
            Vector3 behind = transform.TransformDirection(Vector3.back);
            blackboard["targetDistance"] = localPos.magnitude;
            blackboard["close"] = localPos.magnitude < 12;
            blackboard["targetInFront"] = heading.z > 0;
            blackboard["targetOnRight"] = heading.x > 0;
            blackboard["targetOffCentre"] = Mathf.Abs(heading.x);
            blackboard["obstacleFront"] = Physics.Raycast(this.transform.position, ahead, 4f);
            blackboard["obstacleBack"] = Physics.Raycast(this.transform.position, behind, 4f);
        }

    }
}