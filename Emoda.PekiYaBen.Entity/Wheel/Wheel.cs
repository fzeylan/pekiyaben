using System;

namespace Emoda.PekiYaBen.Entity.Wheel
{
    public class Wheel
    {
        public string Question1 { get; set; }
        public string Question2 { get; set; }
        public string Question3 { get; set; }
        public string Question4 { get; set; }
        public string Question5 { get; set; }
        public string Question6 { get; set; }
        public string Question7 { get; set; }
        public string Question8 { get; set; }

        public short Score1 { get; set; }
        public short Score2 { get; set; }
        public short Score3 { get; set; }
        public short Score4 { get; set; }
        public short Score5 { get; set; }
        public short Score6 { get; set; }
        public short Score7 { get; set; }
        public short Score8 { get; set; }

        public short DemandScore1 { get; set; }
        public short DemandScore2 { get; set; }
        public short DemandScore3 { get; set; }
        public short DemandScore4 { get; set; }
        public short DemandScore5 { get; set; }
        public short DemandScore6 { get; set; }
        public short DemandScore7 { get; set; }
        public short DemandScore8 { get; set; }

        public string CurrentQuestion { get; set; }
        public short CurrentPoint { get; set; }
        public string CurrentPointMeaning { get; set; }

        public int FuturePoint { get; set; }
        public string FuturePointMeaning { get; set; }
        public int FutureDate { get; set; }

        public string FutureDateType { get; set; }
        public string Resources { get; set; }
        public string ExistingResources { get; set; }
        public string LackingResources { get; set; }

        public int SessionPoint { get; set; }
        public string SessionMeaning { get; set; }
        public bool ReachToPoint { get; set; }

        public bool Overlap { get; set; }
        public bool Realistic { get; set; }
        public bool Restart { get; set; }
        public bool SessionRequest { get; set; }
        public bool DoRealistic { get; set; }
        public bool Flu { get; set; }
        public bool EnoughForNow { get; set; }
    }
}
