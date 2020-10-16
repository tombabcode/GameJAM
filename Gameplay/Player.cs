namespace GameJAM.Gameplay {
    public sealed class Player {

        public float Weight { get; private set; }

        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Tiredness { get; set; }

        public Player(float weight) {
            Weight = weight;
        }

    }
}