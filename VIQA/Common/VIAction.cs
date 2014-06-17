namespace VIQA
{
    public class VIAction<T> where T : class
    {
        public T DefaultAction;
        public VIAction(T defaultAction) { DefaultAction = defaultAction; }

        private T _getElementTextFunc;
        public T Action
        {
            set { _getElementTextFunc = value; }
            get { return _getElementTextFunc ?? DefaultAction; }
        }
    }
}
