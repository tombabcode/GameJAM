namespace GameJAM.Gameplay {
    public sealed class Player {

        public float MaxWeight { get; private set; }

        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Tiredness { get; set; }

        public Player(float maxWeight) {
            MaxWeight = maxWeight;
        }

    }
}