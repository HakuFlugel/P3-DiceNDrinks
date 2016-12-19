namespace Shared {
    public class Room {
        public string name;
        public int seats;

        public override bool Equals(object obj) {
            return name == (obj as Room)?.name;
        }
    }
}