using System;

namespace Assets.Model {
    public enum EntityType {
        Invalid = -1,
        BurgerObstacle,
        HangingBeamObstacle,
        HoleObstacle,
        PillarObstacle,
        TunnelObstacle,
        Target
    }

    [Serializable]
    public class Entity {
        public EntityType Type;

        public float XPosition;
        public float YPosition;
        public float ZPosition;

        public float XRotation;
        public float YRotation;
        public float ZRotation;
        public float WValue;

        public float XScale;
        public float YScale;
        public float ZScale;

        public int Id;
        
        public static EntityType ToEntityType(string entityName){
            switch(entityName){
                case "BurgerObstacle":
                    return EntityType.BurgerObstacle;
                case "HangingBeamObstacle":
                    return EntityType.HangingBeamObstacle;
                case "HoleObstacle":
                    return EntityType.HoleObstacle;
                case "PillarObstacle":
                    return EntityType.PillarObstacle;
                case "Target":
                    return EntityType.Target;
                case "TunnelObstacle":
                    return EntityType.TunnelObstacle;
                default:
                    return EntityType.Invalid;
            }
        }
    }
}