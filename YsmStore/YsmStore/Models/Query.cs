using System.Collections;

namespace YsmStore.Models
{
    public class Query : YsmStoreModel
    {
        private IList _targetList;
        public IList TargetList
        {
            get => _targetList;
            set { _targetList = value; InvokePropertyChanged(nameof(TargetList)); }
        }

        private int _loadStep = 10;
        public int LoadStep
        {
            get => _loadStep;
            set
            {
                if (value <= 0)
                    throw new YsmStoreException("LoadStep must be more than 0");

                _loadStep = value;
                InvokePropertyChanged(nameof(LoadStep));
            }
        }

        public bool IsEndReached { get; set; } = false;
    }
}
