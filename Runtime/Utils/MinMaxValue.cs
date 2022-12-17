using UnityEngine;


namespace Library
{
    [System.Serializable]
    public struct MinMaxInt
    {
        public int min;
        public int max;


        public MinMaxInt(int min, int max)
        {
            this.min = min;
            this.max = max;
        }


        public int Lerp(float t)
        {
            return (int)Mathf.Lerp(min, max, t);
        }


        public int Random()
        {
            return UnityEngine.Random.Range(min, max);
        }
    }


    [System.Serializable]
    public struct MinMaxFloat
    {
        public float min;
        public float max;

        public MinMaxFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }


        public float Lerp(float t)
        {
            return Mathf.Lerp(min, max, t);
        }


        public float Random()
        {
            return UnityEngine.Random.Range(min, max);
        }
    }


    [System.Serializable]
    public struct MinMaxVector2
    {
        public Vector2 min;
        public Vector2 max;

        public MinMaxVector2(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
        }


        public Vector2 Lerp(float t)
        {
            return Vector2.Lerp(min, max, t);
        }
    }

    [System.Serializable]
    public struct MinMaxVector2Int
    {
        public Vector2Int min;
        public Vector2Int max;

        public MinMaxVector2Int(Vector2Int min, Vector2Int max)
        {
            this.min = min;
            this.max = max;
        }
    }


    [System.Serializable]
    public struct MinMaxVector3
    {
        public Vector3 min;
        public Vector3 max;


        public MinMaxVector3(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }


        public Vector3 Lerp(float t)
        {
            return Vector3.Lerp(min, max, t);
        }

        public Vector3 Random()
        {
            return new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
        }
    }


    [System.Serializable]
    public struct MinMaxColor32
    {
        public Color32 min;
        public Color32 max;

        public MinMaxColor32(Color32 min, Color32 max)
        {
            this.min = min;
            this.max = max;
        }


        public Color32 Lerp(float t)
        {
            return Color32.Lerp(min, max, t);
        }
    }


    [System.Serializable]
    public struct MinMaxColor
    {
        public Color min;
        public Color max;


        public MinMaxColor(Color min, Color max)
        {
            this.min = min;
            this.max = max;
        }


        public Color Lerp(float t)
        {
            return Color.Lerp(min, max, t);
        }
    }


    [System.Serializable]
    public struct MinMaxIntCurveValue
    {
        public int min;
        public int max;


        public int roundValue;
        public AnimationCurve curve;


        public MinMaxIntCurveValue(int min, int max, int roundValue, AnimationCurve curve)
        {
            this.min = min;
            this.max = max;
            this.roundValue = roundValue;
            this.curve = curve;
        }


        public int Lerp(float t)
        {
            return Mathf.FloorToInt(Mathf.Lerp(min, max, curve.Evaluate(t)));
        }
    }


    [System.Serializable]
    public struct MinMaxFloatCurveValue
    {
        public float min;
        public float max;

        public AnimationCurve curve;


        public MinMaxFloatCurveValue(float min, float max, AnimationCurve curve)
        {
            this.min = min;
            this.max = max;
            this.curve = curve;
        }

        public float Lerp(float t)
        {
            return Mathf.Lerp(min, max, curve.Evaluate(t));
        }
    }
}