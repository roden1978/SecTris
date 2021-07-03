    public class SectorsIndex: ISectorsIndex
    {
        private readonly float _angel;
        private const int Angel72 = 72;
        private const int Angel144 = 144; 
        private const int Angel216 = 216;
        private const int Angel288 = 288;

        public SectorsIndex(float angel)
        {
            _angel = angel;
        }
        public int Value()
        {
            var index = _angel switch
            {
                Angel72 => 1,
                Angel144 => 2,
                Angel216 => 3,
                Angel288 => 4,
                _ => 0
            };

            return index;
        }
    }

    

    