using obs_test.Application.Extensions;
using obs_test.Domain.Entities;
using obs_test.Domain.Models;

namespace obs_test.Application
{
    public class Robot : IRobot
    {
        public Robot(InputData inputData)
        {
            InputData = inputData;
            RemainingBattery = inputData.Battery;
            CurrentPosition = inputData.InitialPosition;
            VisitedCells = new List<Location>();
            SamplesCollected = new List<string>();
        }
        private InputData InputData { get; set; }
        private Position CurrentPosition { get; set; }
        private int RemainingBattery { get; set; }
        private List<Location> VisitedCells { get; set; }
        private List<string> SamplesCollected { get; set; }
        public  void ExecuteCommand(string command)
        {
            switch (command)
            {
                case "F":
                    MoveForward();
                    break;
                case "B":
                    MoveBackwards();
                    break;
                case "L":
                    TurnLeft();
                    break;
                case "R":
                    TurnRight();
                    break;
                case "S":
                    TakeSample();
                    break;
                case "E":
                    RemainingBattery += 10;
                    RemainingBattery -= 1;
                    break;
            }
        }
        private void ConsumeBattery(int amount)
        {
            RemainingBattery -= amount;
            RemainingBattery = Math.Max(RemainingBattery, 0);
        }
        private void MoveForward()
        {
            var newPositon = CurrentPosition.CalculateNewPosition();
            if (newPositon.IsValidLocation(InputData.Terrain))
                CurrentPosition = newPositon;
            VisitedCells.Add(CurrentPosition.Location);
            ConsumeBattery(3);
        }

        private void MoveBackwards()
        {
            CurrentPosition.Facing = CurrentPosition.Facing.GetOppositeDirection();
            var newPositon = CurrentPosition.CalculateNewPosition();
            if (newPositon.IsValidLocation(InputData.Terrain))
                CurrentPosition = newPositon;
            VisitedCells.Add(CurrentPosition.Location);
            ConsumeBattery(3);
        }

        private void TurnLeft()
        {
            CurrentPosition.Facing = CurrentPosition.Facing.GetLeftTurnDirection();

            ConsumeBattery(2);
        }

        private void TurnRight()
        {
            CurrentPosition.Facing = CurrentPosition.Facing.GetRightTurnDirection();
            ConsumeBattery(2);
        }

        private void TakeSample()
        {
            string sample = InputData.Terrain[CurrentPosition.Location.Y][CurrentPosition.Location.X];
            SamplesCollected.Add(sample);
            ConsumeBattery(8);
        }
        public SimulationResult RunSimulation()
        {
            foreach (string command in InputData.Commands)
            {
                ExecuteCommand(command);
                if (RemainingBattery <= 0)
                    break;
            }

            return new SimulationResult
            {
                VisitedCells = VisitedCells,
                SamplesCollected = SamplesCollected,
                Battery = RemainingBattery,
                FinalPosition = CurrentPosition
            };
        }
        //public SimulationResult RunSimulation(InputData input)
        //{
        //    var visitedCells = new List<Location>();
        //    var samplesCollected = new List<string>();
        //    int remainingBattery = input.Battery;
        //    var currentLocation = input.InitialPosition;
        //    foreach (string command in input.Commands)
        //    {
        //        if (remainingBattery <= 0)
        //            break;

        //        switch (command)
        //        {
        //            case "F":
        //                Location move = MovementLookup[currentLocation.Facing];
        //                Location nextLocation = new()
        //                {
        //                    X = currentLocation.Location.X + move.X,
        //                    Y = currentLocation.Location.Y + move.Y
        //                };

        //                if (nextLocation.IsValidLocation(input.Terrain))
        //                {
        //                    visitedCells.Add(nextLocation);
        //                    currentLocation.Location = nextLocation;
        //                    remainingBattery -= 3;
        //                }
        //                break;
        //            case "B":
        //                Location backMove = MovementLookup[currentLocation.Facing.GetOppositeDirection()];
        //                Location backLocation = new()
        //                {
        //                    X = currentLocation.Location.X + backMove.X,
        //                    Y = currentLocation.Location.Y + backMove.Y
        //                };

        //                if (backLocation.IsValidLocation(input.Terrain))
        //                {
        //                    visitedCells.Add(backLocation);
        //                    currentLocation.Location = backLocation;
        //                    remainingBattery -= 3;
        //                }
        //                break;
        //            case "L":
        //                currentLocation.Facing = currentLocation.Facing.GetLeftTurnDirection();
        //                remainingBattery -= 2;
        //                break;

        //            case "R":
        //                currentLocation.Facing = currentLocation.Facing.GetRightTurnDirection();
        //                remainingBattery -= 2;
        //                break;
        //            case "S":
        //                string material = input.Terrain[currentLocation.Location.Y][currentLocation.Location.X];
        //                samplesCollected.Add(material);
        //                remainingBattery -= 8;
        //                break;
        //            case "E":
        //                remainingBattery += 10;
        //                remainingBattery -= 1;
        //                break;
        //            default:
        //                Console.WriteLine($"Unknown command: {command}");
        //                break;
        //        }
        //    }

        //    SimulationResult result = new()
        //    {
        //        VisitedCells = visitedCells,
        //        SamplesCollected = samplesCollected,
        //        Battery = remainingBattery,
        //        FinalLocation = currentLocation
        //    };

        //    return result;
        //}
    }
}
