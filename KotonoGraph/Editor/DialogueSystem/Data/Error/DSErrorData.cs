using UnityEngine;

namespace DS.Error
{
    public class DSErrorData
    {

        public DSErrorData()
        {
            GenerateRandmoColor();
        }
        
        public Color Color { get; set; }

        private void GenerateRandmoColor()
        {
            Color = new Color32
            (
                (byte)Random.Range(65,266),
                (byte)Random.Range(50,176),
                (byte)Random.Range(50,176),
                255
            );

        }
    }
}
