using UnityEngine;

namespace Game.Miscs
{


    [System.Serializable]
    public sealed class MetaInfo
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;


        public MetaInfo(string name, string description, Sprite icon)
        {
            _name = name;
            _description = description;
            _icon = icon;
        }


        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
    }
}
