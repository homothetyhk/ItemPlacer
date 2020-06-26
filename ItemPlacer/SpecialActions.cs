using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemPlacer
{
    public class DefaultGameObjectInfo
    {
        public string deployOneXLabel = "Deploy x-coordinate";
        public string deployOneYLabel = "Deploy y-coordinate";
        public string deployTwoXLabel = string.Empty;
        public string deployTwoYLabel = string.Empty;
        public string paramOneLabel = string.Empty;
        public string paramTwoLabel = string.Empty;
        public bool useDeployTwo = false;
        public bool useParamOne = false;
        public bool useParamTwo = false;

        public static Dictionary<string, DefaultGameObjectInfo> defaultObjects = new Dictionary<string, DefaultGameObjectInfo>
        {
            {"Platform", new DefaultGameObjectInfo()},
            {"Vengefly", new DefaultGameObjectInfo
            {
                useParamOne = true,
                paramOneLabel = "HP"
            } },
            {"Baldur", new DefaultGameObjectInfo
            {
                useParamOne = true,
                paramOneLabel = "HP",
                useParamTwo = true,
                paramTwoLabel = "Facing right? true/false"
            } },
            {"Toll Gate", new DefaultGameObjectInfo
            {
                deployOneXLabel = "Toll machine x-coordinate",
                deployOneYLabel = "Toll machine y-coordinate",
                useDeployTwo = true,
                deployTwoXLabel = "Gate x-coordinate",
                deployTwoYLabel = "Gate y-coordinate",
                useParamOne = true,
                paramOneLabel = "Toll cost"
            } },
            {"Switch Gate", new DefaultGameObjectInfo
            {
                deployOneXLabel = "Gate switch x-coordinate",
                deployOneYLabel = "Gate switch y-coordinate",
                useDeployTwo = true,
                deployTwoXLabel = "Gate x-coordinate",
                deployTwoYLabel = "Gate y-coordinate",
            } },
            {"Quake Floor", new DefaultGameObjectInfo
            {
                useParamOne = true,
                paramOneLabel = "Disable if entrygate is: (e.g. bot1, etc)"
            } },
            {"Shadow Gate", new DefaultGameObjectInfo() },
            // too much fsm editing
            //{"Elegant Key Gate", new DefaultGameObjectInfo() },
            //{"Love Key Gate", new DefaultGameObjectInfo() },
            //{"King's Brand Gate", new DefaultGameObjectInfo() },
            //{"Simple Key Hatch", new DefaultGameObjectInfo() }
        };

    }

    public class AddSpecialGameObjectAction
    {
        public string specialName;
        public string deploySceneName;
        public float deployOneX;
        public float deployOneY;
        public float deployTwoX;
        public float deployTwoY;
        public string paramOne;
        public string paramTwo;
        public override string ToString()
        {
            return $"Add {specialName} in {deploySceneName} at ({deployOneX}, {deployOneY})";
        }
    }

    public class AddPreloadedGameObjectAction
    {
        public string originalSceneName;
        public string originalObjectName;
        public string deploySceneName;
        public float deployX;
        public float deployY;
        public override string ToString()
        {
            return $"Add {originalObjectName} in {deploySceneName} at ({deployX}, {deployY})";
        }
    }

    public class DestroyGameObjectAction
    {
        public string originalSceneName;
        public string originalObjectName;
        public bool destroyAllThatMatch;

        public override string ToString()
        {
            if (destroyAllThatMatch)
            {
                return $"Destroy all objects matching {originalObjectName} in {originalSceneName}";
            }

            return $"Destroy {originalObjectName} in {originalSceneName}";
        }
    }

    public class OverrideDarknessAction
    {
        public string sceneName;
        public int darknessLevel;

        public override string ToString()
        {
            return $"Set {sceneName} darkness level to {darknessLevel}";
        }
    }
}
